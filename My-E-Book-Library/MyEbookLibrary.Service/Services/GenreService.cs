using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Genre;
using MyEbookLibrary.Common.DTO.Language;
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
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GenreService> _logger;
        private readonly IConfiguration _configuration;
        public GenreService(IGenreRepository genreRepository, ILogger<GenreService> logger, IConfiguration configuration, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<DropdownResult<GenreDropdownListDTO>> GetGenreDropdownList(string? search, UserDTO user)
        {
            try
            {
                int recordLimit = Convert.ToInt32(_configuration["AutocompleteRecordLimit"]);
                search = search?.Trim().ToLower();
                Specification<Genre> specification = new Specification<Genre>();
                specification.AddCriteria(d => (d.AddedBy == user.Id || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted && (string.IsNullOrEmpty(search) || d.GenreName.ToLower().Contains(search)));
                specification.AddOrderBy(d => d.GenreName);

                var genres = await _genreRepository.GetBySpecificationAsync(specification, recordLimit);
                List<GenreDropdownListDTO> list = new List<GenreDropdownListDTO>();
                foreach (var genre in genres)
                {
                    list.Add(new GenreDropdownListDTO(genre.Id, genre.GenreName));
                }
                return DropdownResult<GenreDropdownListDTO>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the genre dropdown list by user id - {0}", user.Id);
            }

            return DropdownResult<GenreDropdownListDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<ListResult<GenreListDTO>> GetGenreList(ListRequestDTO listRequest, UserDTO user)
        {
            try
            {
                if (listRequest != null)
                {
                    string searchText = !string.IsNullOrEmpty(listRequest.SearchText) ? listRequest.SearchText.Trim().ToLower() : string.Empty;

                    Specification<Genre> specification = new Specification<Genre>();
                    specification.AddCriteria(genre =>
                                        (genre.AddedBy == user.Id || genre.AddedBy == Constants.SYSTEM_USER_ID) &&
                                        !genre.Deleted &&
                                        (string.IsNullOrEmpty(searchText) ||
                                        genre.GenreName.ToLower().Contains(searchText)));
                    specification.AddIncludes([(genre => genre.Books.Where(b => !b.Deleted))]);

                    var genres = await _genreRepository.GetBySpecificationAsync(specification);
                    if (genres != null && genres.Count > 0)
                    {
                        int totalCount = genres.Count();
                        if (totalCount > 0 && totalCount > listRequest.PageSize * listRequest.PageNumber)
                        {
                            List<GenreListDTO> list = new List<GenreListDTO>();
                            foreach (var genre in genres)
                            {
                                list.Add(new GenreListDTO(genre.Id, genre.GenreName, genre.Books.Count, genre.AddedBy));
                            }
                            var records = Helper.SortAndPage(list, listRequest.PageSize, listRequest.PageNumber, string.IsNullOrEmpty(listRequest.SortColumnName) ? "GenreName" : listRequest.SortColumnName, listRequest.SortBy);
                            return ListResult<GenreListDTO>.Success(records, totalCount);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the genre list by user id - {0}", user.Id);
            }
            return ListResult<GenreListDTO>.Failure(Error.RecordNotFound()); 
        }

        public async Task<bool> DeleteGenre(int id, UserDTO user)
        {
            bool retVal = false;
            try
            {
                if (id > 0)
                {
                    var genre = await _genreRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (genre != null)
                    {
                        await _genreRepository.DeleteAsync(id);
                        retVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting the genre with id - {0} by user id - {1}", id, user.Id);
            }
            return retVal;
        }

        public async Task<DetailResult<GenreDTO>> GetGenre(int id, UserDTO user)
        {
            try
            {
                if (id > 0)
                {
                    var genre = await _genreRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (genre != null)
                    {
                        var dto = _mapper.Map<GenreDTO>(genre);
                        return DetailResult<GenreDTO>.Success(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the genre detail for id - {0} by user id - {1}", id, user.Id);
            }
            return DetailResult<GenreDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<SaveResult<GenreDTO>> SaveGenre(GenreDTO genre, UserDTO user)
        {
            if (genre == null)
            {
                return SaveResult<GenreDTO>.Failure(Error.ValidationError("Null Genre argument passed for saving."));
            }
            try
            {
                DateTime dateTime = DateTime.UtcNow;
                Genre? entity = null;
                if (genre != null)
                {
                    if (genre.Id > 0)
                    {
                        entity = await _genreRepository.GetAsync(genre.Id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                        if (entity != null)
                        {
                            entity.GenreName = genre.GenreName;
                        }
                        else
                        {
                            return SaveResult<GenreDTO>.Failure(Error.ValidationError("No genre found to save."));
                        }
                    }
                    else
                    {
                        entity = new Genre(genre.GenreName);
                        entity.AddedDate = dateTime;
                        entity.AddedBy = user.Id;
                    }
                    entity.LastUpdatedDate = dateTime;

                    if (entity.Id == 0)
                    {
                        entity = await _genreRepository.AddAsync(entity);
                    }
                    else
                    {
                        entity = await _genreRepository.UpdateAsync(entity);
                    }
                    return SaveResult<GenreDTO>.Success(_mapper.Map<GenreDTO>(entity));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while saving the genre with id - {0} by user id - {1}", genre.Id, user.Id);

            }
            return SaveResult<GenreDTO>.Failure(Error.SaveFailure("Genre couldn't be saved."));
        }
    }
}
