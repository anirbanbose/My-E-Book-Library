using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
using MyEbookLibrary.Common.DTO.Publisher;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Data.Repository.Repositories;
using MyEbookLibrary.Data.Repository.Specifications;
using MyEbookLibrary.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PublisherService> _logger;
        private readonly IConfiguration _configuration;
        public PublisherService(IPublisherRepository publisherRepository, IMapper mapper, ILogger<PublisherService> logger, IConfiguration configuration)
        {
            _publisherRepository = publisherRepository;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<DropdownResult<PublisherDropdownListDTO>> GetPublisherDropdownList(string? search, UserDTO user)
        {
            try
            {
                int recordLimit = Convert.ToInt32(_configuration["AutocompleteRecordLimit"]);
                search = search?.Trim().ToLower();
                Specification<Publisher> specification = new Specification<Publisher>();
                specification.AddCriteria(d => (d.AddedBy == user.Id || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted && (string.IsNullOrEmpty(search) || d.PublisherName.ToLower().Contains(search)));
                specification.AddOrderBy(d => d.PublisherName);
                
                var publishers = await _publisherRepository.GetBySpecificationAsync(specification, recordLimit);
                
                List<PublisherDropdownListDTO> list = new List<PublisherDropdownListDTO>();
                foreach (var publisher in publishers)
                {
                    list.Add(new PublisherDropdownListDTO(publisher.Id, publisher.PublisherName));
                }
                return DropdownResult<PublisherDropdownListDTO>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the publisher dropdown list by user id - {0}", user.Id);
            }

            return DropdownResult<PublisherDropdownListDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<ListResult<PublisherListDTO>> GetPublisherList(ListRequestDTO listRequest, UserDTO user)
        {
            try
            {
                if (listRequest != null)
                {
                    string searchText = !string.IsNullOrEmpty(listRequest.SearchText) ? listRequest.SearchText.Trim().ToLower() : string.Empty;

                    Specification<Publisher> specification = new Specification<Publisher>();
                    specification.AddCriteria(publisher =>
                                        (publisher.AddedBy == user.Id || publisher.AddedBy == Constants.SYSTEM_USER_ID) &&
                                        !publisher.Deleted &&
                                        (string.IsNullOrEmpty(searchText) ||
                                        publisher.PublisherName.ToLower().Contains(searchText)));
                    specification.AddIncludes([(publisher => publisher.Books.Where(b => !b.Deleted))]);

                    var publishers = await _publisherRepository.GetBySpecificationAsync(specification);
                    if (publishers != null && publishers.Count > 0)
                    {
                        int totalCount = publishers.Count();
                        if (totalCount > 0 && totalCount > listRequest.PageSize * listRequest.PageNumber)
                        {
                            List<PublisherListDTO> list = new List<PublisherListDTO>();
                            foreach (var publisher in publishers)
                            {
                                list.Add(new PublisherListDTO(publisher.Id, publisher.PublisherName, publisher.Books.Count, publisher.AddedBy));
                            }
                            var records = Helper.SortAndPage(list, listRequest.PageSize, listRequest.PageNumber, string.IsNullOrEmpty(listRequest.SortColumnName) ? "PublisherName" : listRequest.SortColumnName, listRequest.SortBy);

                            return ListResult<PublisherListDTO>.Success(records, totalCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the publisher list by user id - {0}", user.Id);
            }

            return ListResult<PublisherListDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<DetailResult<PublisherDTO>> GetPublisher(int id, UserDTO user)
        {
            try
            {
                if (id > 0)
                {
                    var publisher = await _publisherRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (publisher != null)
                    {
                        var dto = _mapper.Map<PublisherDTO>(publisher);
                        return DetailResult<PublisherDTO>.Success(dto);
                    }
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Exception occurred while retrieving the publisher detail for id - {0} by user id - {1}", id, user.Id);
            }
            return DetailResult<PublisherDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<bool> DeletePublisher(int id, UserDTO user)
        {
            bool retVal = false;
            try
            {
                if (id > 0)
                {
                    var publisher = await _publisherRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (publisher != null)
                    {
                        await _publisherRepository.DeleteAsync(id);
                        retVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting the publisher with id - {0} by user id - {1}", id, user.Id);
            }           

            return retVal;
        }

        public async Task<SaveResult<PublisherDTO>> SavePublisher(PublisherDTO publisher, UserDTO user)
        {
            if (publisher == null)
            {
                return SaveResult<PublisherDTO>.Failure(Error.ValidationError("Null Publisher argument passed for saving."));
            }
            try
            {
                DateTime dateTime = DateTime.UtcNow;
                Publisher? entity = null;
                if(publisher != null)
                {
                    if (publisher.Id > 0)
                    {
                        entity = await _publisherRepository.GetAsync(publisher.Id , d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                        if (entity != null)
                        {
                            entity.PublisherName = publisher.PublisherName;
                        }
                        else
                        {
                            return SaveResult<PublisherDTO>.Failure(Error.ValidationError("No publisher found to save."));
                        }
                    }
                    else
                    {
                        entity = new Publisher(publisher.PublisherName);
                        entity.AddedDate = dateTime;
                        entity.AddedBy = user.Id;
                    }
                    entity.LastUpdatedDate = dateTime;

                    if (entity.Id == 0)
                    {                        
                        entity = await _publisherRepository.AddAsync(entity);
                    }
                    else
                    {
                        entity = await _publisherRepository.UpdateAsync(entity);
                    }
                    return SaveResult<PublisherDTO>.Success(_mapper.Map<PublisherDTO>(entity));
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while saving the publisher with id - {0} by user id - {1}", publisher.Id, user.Id);
                
            }
            return SaveResult<PublisherDTO>.Failure(Error.SaveFailure("Publisher couldn't be saved."));
        }
    }
}
