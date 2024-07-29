using AutoMapper;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
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
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public LanguageService(ILanguageRepository languageRepository, ILogger<LanguageService> logger, IConfiguration configuration, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _logger = logger;
            _configuration = configuration;
            _mapper = mapper;

        }

        public async Task<DropdownResult<LanguageDropdownListDTO>> GetLanguageDropdownList(string? search, UserDTO user)
        {
            try
            {
                int recordLimit = Convert.ToInt32(_configuration["AutocompleteRecordLimit"]);
                search = search?.Trim().ToLower();

                Specification<Language> specification = new Specification<Language>();
                specification.AddCriteria(d => (d.AddedBy == user.Id || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted && (string.IsNullOrEmpty(search) || d.LanguageName.ToLower().Contains(search) || d.LanguageCode.ToLower().Contains(search)));
                specification.AddOrderBy(d => d.LanguageName);

                var languages = await _languageRepository.GetBySpecificationAsync(specification, recordLimit); 
                List<LanguageDropdownListDTO> list = new List<LanguageDropdownListDTO>();
                foreach (var language in languages)
                {
                    list.Add(new LanguageDropdownListDTO(language.Id, language.LanguageName));
                }
                return DropdownResult<LanguageDropdownListDTO>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the language dropdown list by user id - {0}", user.Id);
            }

            return DropdownResult<LanguageDropdownListDTO>.Failure(Error.RecordNotFound());
        }

        public async Task<ListResult<LanguageListDTO>> GetLanguageList(ListRequestDTO listRequest, UserDTO user)
        {
            try
            {
                if (listRequest != null)
                {
                    string searchText = !string.IsNullOrEmpty(listRequest.SearchText) ? listRequest.SearchText.Trim().ToLower() : string.Empty;

                    Specification<Language> specification = new Specification<Language>();
                    specification.AddCriteria(language =>
                                        (language.AddedBy == user.Id || language.AddedBy == Constants.SYSTEM_USER_ID) &&
                                        !language.Deleted &&
                                        (string.IsNullOrEmpty(searchText) ||
                                        language.LanguageName.ToLower().Contains(searchText) ||
                                        language.LanguageCode.ToLower().Contains(searchText)));
                    specification.AddIncludes([(language => language.Books.Where(b => !b.Deleted))]);     
                    
                    var languages = await _languageRepository.GetBySpecificationAsync(specification);
                    if (languages != null && languages.Count > 0)
                    {
                        int totalCount = languages.Count();
                        if (totalCount > 0 && totalCount > listRequest.PageSize * listRequest.PageNumber)
                        {
                            List<LanguageListDTO> list = new List<LanguageListDTO>();
                            foreach (var language in languages)
                            {
                                list.Add(new LanguageListDTO(language.Id, language.LanguageName, language.LanguageCode.ToUpper(), language.Books.Count, language.AddedBy));
                            }
                            var records = Helper.SortAndPage(list, listRequest.PageSize, listRequest.PageNumber, string.IsNullOrEmpty(listRequest.SortColumnName) ? "LanguageName" : listRequest.SortColumnName, listRequest.SortBy);
                            return ListResult<LanguageListDTO>.Success(records, totalCount);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the language list by user id - {0}", user.Id);
            }
            
            return ListResult<LanguageListDTO>.Failure(Error.RecordNotFound()); ;
        }

        public async Task<DetailResult<LanguageDTO>> GetLanguage(int id, UserDTO user)
        {
            try
            {
                if (id > 0)
                {
                    var language = await _languageRepository.GetAsync(id, d => d.AddedBy == user.Id || (d.AddedBy == Constants.SYSTEM_USER_ID && user.IsAdmin));
                    if (language != null)
                    {
                        var dto = _mapper.Map<LanguageDTO>(language);
                        return DetailResult<LanguageDTO>.Success(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the language detail for id - {0} by user id - {1}", id, user.Id);
            }
            return DetailResult<LanguageDTO>.Failure(Error.RecordNotFound());
        }
    }
}
