using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Author;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;
        private readonly IConfiguration _configuration;
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper, ILogger<AuthorService> logger, IConfiguration configuration, UserManager<User> userManager) 
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<DropdownResult<AuthorDropdownListDTO>> GetAuthorDropdownList(string? search, UserDTO user)
        {
            try
            {
                int recordLimit = Convert.ToInt32(_configuration["AutocompleteRecordLimit"]);
                search = search?.Trim().ToLower();

                Specification<Author> specification = new Specification<Author>();
                specification.AddCriteria(d => !d.Deleted && (d.AddedBy == user.Id || d.AddedBy == Constants.SYSTEM_USER_ID) && (string.IsNullOrEmpty(search) || d.FirstName.ToLower().Contains(search) || d.LastName.ToLower().Contains(search) || (!string.IsNullOrEmpty(d.MiddleName) && d.MiddleName.ToLower().Contains(search))));
                specification.AddOrderBy(d => d.FirstName);

                var authors = await _authorRepository.GetBySpecificationAsync(specification, recordLimit);
                
                List<AuthorDropdownListDTO> list = new List<AuthorDropdownListDTO>();
                foreach (var author in authors)
                {
                    list.Add(new AuthorDropdownListDTO(author.Id, author.FirstName, author.MiddleName, author.LastName));
                }
                return DropdownResult<AuthorDropdownListDTO>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the author dropdown list by user id - {0}", user.Id);
            }

            return DropdownResult<AuthorDropdownListDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<ListResult<AuthorListDTO>> GetAuthorList(ListRequestDTO listRequest, UserDTO user)
        {            
            try
            {
                if (listRequest != null)
                {
                    string searchText = !string.IsNullOrEmpty(listRequest.SearchText) ? listRequest.SearchText.Trim().ToLower() : string.Empty;

                    
                    Expression<Func<Author, bool>> condition = author =>
                                        (author.AddedBy == user.Id || author.AddedBy == Constants.SYSTEM_USER_ID) &&
                                        !author.Deleted &&
                                        (string.IsNullOrEmpty(searchText) || 
                                        author.FirstName.ToLower().Contains(searchText) ||
                                        author.LastName.ToLower().Contains(searchText) ||
                                        (!string.IsNullOrEmpty(author.MiddleName) && author.MiddleName.ToLower().Contains(searchText)));
                    
                    
                    var authors = await _authorRepository.GetByCriteriaAsync(condition);
                    if (authors != null && authors.Count > 0)
                    {                        
                        int totalCount = authors.Count();
                        if (totalCount > 0 && totalCount > listRequest.PageSize * listRequest.PageNumber)
                        {                            
                            List<AuthorListDTO> list = new List<AuthorListDTO>();
                            foreach (var author in authors)
                            {
                                list.Add(new AuthorListDTO(author.Id, author.FirstName, author.MiddleName, author.LastName, author.BookAuthors.Select(ba => ba.Book).Count(), author.AddedBy));
                            }

                            var records = Helper.SortAndPage(list, listRequest.PageSize, listRequest.PageNumber, string.IsNullOrEmpty(listRequest.SortColumnName) ? "AuthorName" : listRequest.SortColumnName, listRequest.SortBy);
                            
                            return ListResult<AuthorListDTO>.Success(records, totalCount);                        
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the author list by user id - {0}", user.Id);
            }
            
            return ListResult<AuthorListDTO>.Failure(Error.RecordNotFound()); 
        }


        public async Task<DetailResult<AuthorDetailDTO>> GetAuthor(int id, UserDTO user)
        {
            try
            {
                if (id > 0)
                {
                    var author = await _authorRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (author != null)
                    {
                        var dto = _mapper.Map<AuthorDetailDTO>(author);
                        return DetailResult<AuthorDetailDTO>.Success(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the author detail for id - {0} by user id - {1}", id, user.Id);
            }
            return DetailResult<AuthorDetailDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<bool> DeleteAuthor(int id, UserDTO user)
        {
            bool retVal = false;
            try
            {
                if (id > 0)
                {
                    var author = await _authorRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (author != null)
                    {
                        await _authorRepository.DeleteAsync(id);
                        retVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting the author with id - {0} by user id - {1}", id, user.Id);
            }

            return retVal;
        }

        public async Task<SaveResult<AuthorDTO>> SaveAuthor(AuthorDTO author, UserDTO user)
        {
            if (author == null)
            {
                return SaveResult<AuthorDTO>.Failure(Error.ValidationError("Null Author argument passed for saving."));
            }
            try
            {
                DateTime dateTime = DateTime.UtcNow;
                Author? entity = null;
                if (author != null)
                {
                    if (author.Id > 0)
                    {
                        entity = await _authorRepository.GetAsync(author.Id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                        if (entity != null)
                        {
                            entity.FirstName = author.FirstName;
                            entity.MiddleName = author.MiddleName;
                            entity.LastName = author.LastName;
                        }
                        else
                        {
                            return SaveResult<AuthorDTO>.Failure(Error.ValidationError("No author found to save."));
                        }
                    }
                    else
                    {
                        entity = new Author(author.FirstName, author.LastName);
                        entity.MiddleName= author.MiddleName;
                        entity.AddedDate = dateTime;
                        entity.AddedBy = user.Id;
                    }
                    entity.LastUpdatedDate = dateTime;

                    if (entity.Id == 0)
                    {
                        entity = await _authorRepository.AddAsync(entity);
                    }
                    else
                    {
                        entity = await _authorRepository.UpdateAsync(entity);
                    }

                    return SaveResult<AuthorDTO>.Success(_mapper.Map<AuthorDTO>(entity));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while saving the author with id - {0} by user id - {1}", author.Id, user.Id);
            }
            return SaveResult<AuthorDTO>.Failure(Error.SaveFailure("Author couldn't be saved."));
        }
    }
    
}
