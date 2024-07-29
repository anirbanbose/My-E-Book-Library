using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyEbookLibrary.Common.DTO;
using MyEbookLibrary.Data.EF;
using MyEbookLibrary.Data.EF.Entities;
using MyEbookLibrary.Data.Repository.Contracts;
using MyEbookLibrary.Data.Repository.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Data.Repository.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
