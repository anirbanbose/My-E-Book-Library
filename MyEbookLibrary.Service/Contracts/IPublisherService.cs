using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Publisher;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IPublisherService
    {
        Task<ListResult<PublisherListDTO>> GetPublisherList(ListRequestDTO listRequest, UserDTO user);
        Task<bool> DeletePublisher(int id, UserDTO user);
        Task<SaveResult<PublisherDTO>> SavePublisher(PublisherDTO publisher, UserDTO user);
        Task<DetailResult<PublisherDTO>> GetPublisher(int id, UserDTO user);
        Task<DropdownResult<PublisherDropdownListDTO>> GetPublisherDropdownList(string? search, UserDTO user);
    }
}
