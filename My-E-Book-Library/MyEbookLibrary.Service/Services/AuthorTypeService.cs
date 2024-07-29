using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyEbookLibrary.Common;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.AuthorType;
using MyEbookLibrary.Common.DTO.Responses;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Data.Repository.Specifications;
using MyEbookLibrary.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Services
{
    public class AuthorTypeService : IAuthorTypeService
    {
        private readonly IAuthorTypeRepository _authorTypeRepository;
        private readonly ILogger<AuthorTypeService> _logger;
        private readonly IConfiguration _configuration;
        public AuthorTypeService(IAuthorTypeRepository authorTypeRepository, ILogger<AuthorTypeService> logger, IConfiguration configuration)
        {
            _authorTypeRepository = authorTypeRepository;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<DropdownResult<AuthorTypeDropdownListDTO>> GetAuthorTypeDropdownList(UserDTO user)
        {
            try
            {
                Specification<AuthorType> specification = new Specification<AuthorType>();
                specification.AddCriteria(d => (d.AddedBy == user.Id || d.AddedBy == Constants.SYSTEM_USER_ID) && !d.Deleted);
                specification.AddOrderBy(d => d.TypeName);
                
                var authorTypes = await _authorTypeRepository.GetBySpecificationAsync(specification);
                List<AuthorTypeDropdownListDTO> list = new List<AuthorTypeDropdownListDTO>();
                foreach (var authorType in authorTypes)
                {
                    list.Add(new AuthorTypeDropdownListDTO(authorType.Id, authorType.TypeName));
                }
                return DropdownResult<AuthorTypeDropdownListDTO>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving the Author Type dropdown list by user id - {0}", user.Id);
            }

            return DropdownResult<AuthorTypeDropdownListDTO>.Failure(Error.RecordNotFound());
        }
    }
}
