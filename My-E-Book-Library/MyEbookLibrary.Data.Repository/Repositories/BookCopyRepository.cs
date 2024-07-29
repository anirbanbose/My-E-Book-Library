using MyEbookLibrary.Data.EF;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Data.Repository.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Repositories
{
    public class BookCopyRepository : BaseRepository<BookCopy>, IBookCopyRepository
    {
        public BookCopyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
