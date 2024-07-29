using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Common.DTO.Account;
using MyEbookLibrary.Common.DTO.Book;
using MyEbookLibrary.Common.DTO.Requests;
using MyEbookLibrary.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Service.Contracts
{
    public interface IBookService
    {
        Task<ListResult<BookListDTO>> GetBookList(BookSearchRequestDTO listRequest, UserDTO user);
        Task<SaveResult> SaveBook(SaveBookDTO book, UserDTO user);
        Task<DetailResult<BookDTO>> GetBook(int id, UserDTO user);
        Task<DetailResult<BookCopyDTO>> GetBookCopiesByBookId(int bookId, UserDTO user);
        Task<DetailResult<FileDataDto>> GetFileForDownlaod(int bookCopyId, UserDTO user);
        Task<bool> DeleteBook(int id, UserDTO user);
        Task<DetailResult<BookDetailDTO>> GetBookDetail(int id, UserDTO user);
    }
}
