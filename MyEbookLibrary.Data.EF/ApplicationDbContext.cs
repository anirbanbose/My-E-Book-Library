using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyEbookLibrary.Data.EF.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyEbookLibrary.Common;

namespace MyEbookLibrary.Data.EF
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateAdded = DateTime.UtcNow;
            InitializeIdentityData(modelBuilder, dateAdded);
            InitializeGenreData(modelBuilder, dateAdded);
            InitializeLanguageData(modelBuilder, dateAdded);
            InitializeAuthorTypeData(modelBuilder, dateAdded);
            InitializeAuthorData(modelBuilder, dateAdded);
            InitializePublisherData(modelBuilder, dateAdded);

            base.OnModelCreating(modelBuilder);

            SetForeignKeys(modelBuilder);

            SetTableNameAndSchema(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        #region Entities 

        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<AuthorType> AuthorTypes { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        #endregion

        #region "Foreign Key"

        private void SetForeignKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many RoleClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });


            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.AuthorId, ba.AuthorTypeId, ba.BookId });

            modelBuilder.Entity<Book>(b =>
            {
                b.HasMany(b => b.Languages).WithMany(l => l.Books);

                b.HasMany(b => b.Genres).WithMany(g => g.Books);
            });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.AuthorType)
                .WithMany(at => at.BookAuthors)
                .HasForeignKey(ba => ba.AuthorTypeId)
                .OnDelete(DeleteBehavior.Restrict); // To prevent cascading delete

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

        }

        #endregion

        #region "Table name & Schema"

        private void SetTableNameAndSchema(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User", "EbookLibrary");
            modelBuilder.Entity<Role>().ToTable("Role", "EbookLibrary");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRole", "EbookLibrary");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim", "EbookLibrary");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin", "EbookLibrary");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserToken", "EbookLibrary");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim", "EbookLibrary");
        }

        #endregion

        #region Data Initialization

        private void InitializeGenreData(ModelBuilder modelBuilder, DateTime dateAdded)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre
                {
                    Id = 1,
                    GenreName = "Thriller",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new Genre
                {
                    Id = 2,
                    GenreName = "Fantasy",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 3,
                    GenreName = "Sci-Fi",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 4,
                    GenreName = "History",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 5,
                    GenreName = "Science",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 6,
                    GenreName = "Historical Fiction",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 7,
                    GenreName = "Adventure",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 8,
                    GenreName = "Commedy",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 9,
                    GenreName = "Horror & Mystery",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 10,
                    GenreName = "Romance",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 11,
                    GenreName = "Drama",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 12,
                    GenreName = "Theatre",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 13,
                    GenreName = "Music",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 14,
                    GenreName = "Poetry",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 15,
                    GenreName = "Children's",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 16,
                    GenreName = "Health / Fitness",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 17,
                    GenreName = "Arts & Culture",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 18,
                    GenreName = "Travel",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 19,
                    GenreName = "Fairy Tale",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 20,
                    GenreName = "Crime",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 21,
                    GenreName = "Cooking",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                }, new Genre
                {
                    Id = 22,
                    GenreName = "Classic",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                    
                });
        }

        private void InitializeLanguageData(ModelBuilder modelBuilder, DateTime dateAdded)
        {
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, LanguageName = "Abkhazian", LanguageCode = "ab", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 2, LanguageName = "Afar", LanguageCode = "aa", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 3, LanguageName = "Afrikaans", LanguageCode = "af", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 4, LanguageName = "Akan", LanguageCode = "ak", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 5, LanguageName = "Albanian", LanguageCode = "sq", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 6, LanguageName = "Amharic", LanguageCode = "am", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 7, LanguageName = "Arabic", LanguageCode = "ar", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 8, LanguageName = "Aragonese", LanguageCode = "an", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 9, LanguageName = "Armenian", LanguageCode = "hy", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 10, LanguageName = "Assamese", LanguageCode = "as", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 11, LanguageName = "Avaric", LanguageCode = "av", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 12, LanguageName = "Avestan", LanguageCode = "ae", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 13, LanguageName = "Aymara", LanguageCode = "ay", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 14, LanguageName = "Azerbaijani", LanguageCode = "az", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 15, LanguageName = "Bambara", LanguageCode = "bm", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 16, LanguageName = "Bashkir", LanguageCode = "ba", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 17, LanguageName = "Basque", LanguageCode = "eu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 18, LanguageName = "Belarusian", LanguageCode = "be", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 19, LanguageName = "Bengali", LanguageCode = "bn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 20, LanguageName = "Bislama", LanguageCode = "bi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 21, LanguageName = "Bosnian", LanguageCode = "bs", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 22, LanguageName = "Breton", LanguageCode = "br", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 23, LanguageName = "Bulgarian", LanguageCode = "bg", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 24, LanguageName = "Burmese", LanguageCode = "my", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 25, LanguageName = "Catalan", LanguageCode = "ca", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 26, LanguageName = "Chamorro", LanguageCode = "ch", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 27, LanguageName = "Chechen", LanguageCode = "ce", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 28, LanguageName = "Chewa", LanguageCode = "ny", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 29, LanguageName = "Chinese", LanguageCode = "zh", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 30, LanguageName = "Chuvash", LanguageCode = "cv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 31, LanguageName = "Cornish", LanguageCode = "kw", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 32, LanguageName = "Corsican", LanguageCode = "co", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 33, LanguageName = "Cree", LanguageCode = "cr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 34, LanguageName = "Croatian", LanguageCode = "hr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 35, LanguageName = "Czech", LanguageCode = "cs", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 36, LanguageName = "Danish", LanguageCode = "da", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 37, LanguageName = "Dhivehi", LanguageCode = "dv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 38, LanguageName = "Dutch", LanguageCode = "nl", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 39, LanguageName = "Dzongkha", LanguageCode = "dz", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 40, LanguageName = "English", LanguageCode = "en", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 41, LanguageName = "Esperanto", LanguageCode = "eo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 42, LanguageName = "Estonian", LanguageCode = "et", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 43, LanguageName = "Ewe", LanguageCode = "ee", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 44, LanguageName = "Faroese", LanguageCode = "fo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 45, LanguageName = "Fijian", LanguageCode = "fj", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 46, LanguageName = "Finnish", LanguageCode = "fi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 47, LanguageName = "French", LanguageCode = "fr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 48, LanguageName = "Western Frisian", LanguageCode = "fy", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 49, LanguageName = "Fulah", LanguageCode = "ff", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 50, LanguageName = "Gaelic", LanguageCode = "gd", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 51, LanguageName = "Galician", LanguageCode = "gl", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 52, LanguageName = "Ganda", LanguageCode = "lg", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 53, LanguageName = "Georgian", LanguageCode = "ka", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 54, LanguageName = "German", LanguageCode = "de", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 55, LanguageName = "Greek", LanguageCode = "el", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 56, LanguageName = "Kalaallisut", LanguageCode = "kl", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 57, LanguageName = "Guarani", LanguageCode = "gn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 58, LanguageName = "Gujarati", LanguageCode = "gu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 59, LanguageName = "Haitian", LanguageCode = "ht", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 60, LanguageName = "Hausa", LanguageCode = "ha", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 61, LanguageName = "Hebrew", LanguageCode = "he", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 62, LanguageName = "Herero", LanguageCode = "hz", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 63, LanguageName = "Hindi", LanguageCode = "hi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 64, LanguageName = "Hiri Motu", LanguageCode = "ho", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 65, LanguageName = "Hungarian", LanguageCode = "hu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 66, LanguageName = "Icelandic", LanguageCode = "is", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 67, LanguageName = "Ido", LanguageCode = "io", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 68, LanguageName = "Igbo", LanguageCode = "ig", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 69, LanguageName = "Indonesian", LanguageCode = "id", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 70, LanguageName = "Inuktitut", LanguageCode = "iu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 71, LanguageName = "Inupiaq", LanguageCode = "ik", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 72, LanguageName = "Irish", LanguageCode = "ga", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 73, LanguageName = "Italian", LanguageCode = "it", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 74, LanguageName = "Japanese", LanguageCode = "ja", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 75, LanguageName = "Javanese", LanguageCode = "jv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 76, LanguageName = "Kannada", LanguageCode = "kn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 77, LanguageName = "Kanuri", LanguageCode = "kr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 78, LanguageName = "Kashmiri", LanguageCode = "ks", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 79, LanguageName = "Kazakh", LanguageCode = "kk", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 80, LanguageName = "Central Khmer", LanguageCode = "km", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 81, LanguageName = "Kikuyu", LanguageCode = "ki", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 82, LanguageName = "Kinyarwanda", LanguageCode = "rw", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 83, LanguageName = "Kyrgyz", LanguageCode = "ky", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 84, LanguageName = "Komi", LanguageCode = "kv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 85, LanguageName = "Kongo", LanguageCode = "kg", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 86, LanguageName = "Korean", LanguageCode = "ko", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 87, LanguageName = "Kwanyama", LanguageCode = "kj", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 88, LanguageName = "Kurdish", LanguageCode = "ku", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 89, LanguageName = "Lao", LanguageCode = "lo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 90, LanguageName = "Latin", LanguageCode = "la", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 91, LanguageName = "Latvian", LanguageCode = "lv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 92, LanguageName = "Limburgish", LanguageCode = "li", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 93, LanguageName = "Lingala", LanguageCode = "ln", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 94, LanguageName = "Lithuanian", LanguageCode = "lt", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 95, LanguageName = "Luba-Katanga", LanguageCode = "lu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 96, LanguageName = "Luxembourgish", LanguageCode = "lb", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 97, LanguageName = "Macedonian", LanguageCode = "mk", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 98, LanguageName = "Malagasy", LanguageCode = "mg", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 99, LanguageName = "Malay", LanguageCode = "ms", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 100, LanguageName = "Malayalam", LanguageCode = "ml", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 101, LanguageName = "Maltese", LanguageCode = "mt", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 102, LanguageName = "Manx", LanguageCode = "gv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 103, LanguageName = "Maori", LanguageCode = "mi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 104, LanguageName = "Marathi", LanguageCode = "mr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 105, LanguageName = "Marshallese", LanguageCode = "mh", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 106, LanguageName = "Mongolian", LanguageCode = "mn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 107, LanguageName = "Nauru", LanguageCode = "na", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 108, LanguageName = "Navajo", LanguageCode = "nv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 109, LanguageName = "North Ndebele", LanguageCode = "nd", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 110, LanguageName = "South Ndebele", LanguageCode = "nr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 111, LanguageName = "Ndonga", LanguageCode = "ng", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 112, LanguageName = "Nepali", LanguageCode = "ne", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 113, LanguageName = "Norwegian", LanguageCode = "no", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 114, LanguageName = "Norwegian Bokmål", LanguageCode = "nb", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 115, LanguageName = "Norwegian Nynorsk", LanguageCode = "nn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 116, LanguageName = "Nuosu", LanguageCode = "ii", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 117, LanguageName = "Occitan", LanguageCode = "oc", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 118, LanguageName = "Ojibwa", LanguageCode = "oj", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 119, LanguageName = "Oriya", LanguageCode = "or", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 120, LanguageName = "Oromo", LanguageCode = "om", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 121, LanguageName = "Ossetian, Ossetic", LanguageCode = "os", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 122, LanguageName = "Pali", LanguageCode = "pi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 123, LanguageName = "Pashto", LanguageCode = "ps", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 124, LanguageName = "Persian", LanguageCode = "fa", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 125, LanguageName = "Polish", LanguageCode = "pl", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 126, LanguageName = "Portuguese", LanguageCode = "pt", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 127, LanguageName = "Punjabi", LanguageCode = "pa", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 128, LanguageName = "Quechua", LanguageCode = "qu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 129, LanguageName = "Moldovan", LanguageCode = "ro", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 130, LanguageName = "Romansh", LanguageCode = "rm", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 131, LanguageName = "Rundi", LanguageCode = "rn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 132, LanguageName = "Russian", LanguageCode = "ru", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 133, LanguageName = "Northern Sami", LanguageCode = "se", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 134, LanguageName = "Samoan", LanguageCode = "sm", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 135, LanguageName = "Sango", LanguageCode = "sg", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 136, LanguageName = "Sanskrit", LanguageCode = "sa", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 137, LanguageName = "Sardinian", LanguageCode = "sc", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 138, LanguageName = "Serbian", LanguageCode = "sr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 139, LanguageName = "Shona", LanguageCode = "sn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 140, LanguageName = "Sindhi", LanguageCode = "sd", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 141, LanguageName = "Sinhala", LanguageCode = "si", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 142, LanguageName = "Slovak", LanguageCode = "sk", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 143, LanguageName = "Slovenian", LanguageCode = "sl", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 144, LanguageName = "Somali", LanguageCode = "so", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 145, LanguageName = "Southern Sotho", LanguageCode = "st", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 146, LanguageName = "Spanish", LanguageCode = "es", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 147, LanguageName = "Sundanese", LanguageCode = "su", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 148, LanguageName = "Swahili", LanguageCode = "sw", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 149, LanguageName = "Swati", LanguageCode = "ss", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 150, LanguageName = "Swedish", LanguageCode = "sv", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 151, LanguageName = "Tagalog", LanguageCode = "tl", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 152, LanguageName = "Tahitian", LanguageCode = "ty", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 153, LanguageName = "Tajik", LanguageCode = "tg", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 154, LanguageName = "Tamil", LanguageCode = "ta", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 155, LanguageName = "Tatar", LanguageCode = "tt", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 156, LanguageName = "Telugu", LanguageCode = "te", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 157, LanguageName = "Thai", LanguageCode = "th", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 158, LanguageName = "Tibetan", LanguageCode = "bo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 159, LanguageName = "Tigrinya", LanguageCode = "ti", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 160, LanguageName = "Tonga", LanguageCode = "to", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 161, LanguageName = "Tsonga", LanguageCode = "ts", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 162, LanguageName = "Tswana", LanguageCode = "tn", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 163, LanguageName = "Turkish", LanguageCode = "tr", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 164, LanguageName = "Turkmen", LanguageCode = "tk", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 165, LanguageName = "Twi", LanguageCode = "tw", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 166, LanguageName = "Uyghur", LanguageCode = "ug", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 167, LanguageName = "Ukrainian", LanguageCode = "uk", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 168, LanguageName = "Urdu", LanguageCode = "ur", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 169, LanguageName = "Uzbek", LanguageCode = "uz", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 170, LanguageName = "Venda", LanguageCode = "ve", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 171, LanguageName = "Vietnamese", LanguageCode = "vi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 172, LanguageName = "Volapük", LanguageCode = "vo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 173, LanguageName = "Walloon", LanguageCode = "wa", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 174, LanguageName = "Welsh", LanguageCode = "cy", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 175, LanguageName = "Wolof", LanguageCode = "wo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 176, LanguageName = "Xhosa", LanguageCode = "xh", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 177, LanguageName = "Yiddish", LanguageCode = "yi", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 178, LanguageName = "Yoruba", LanguageCode = "yo", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 179, LanguageName = "Zhuang", LanguageCode = "za", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, },
                new Language { Id = 180, LanguageName = "Zulu", LanguageCode = "zu", Deleted = false, AddedDate = dateAdded, LastUpdatedDate = dateAdded, AddedBy = Constants.SYSTEM_USER_ID, }
            );
        }

        private void InitializeIdentityData(ModelBuilder modelBuilder, DateTime dateAdded)
        {
            User user = new User
            {
                Id = 1,
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                NormalizedEmail = "admin@admin.com",
                NormalizedUserName = "admin@admin.com",
                PhoneNumber = "0",
                SecurityStamp = Guid.NewGuid().ToString(),
                Deleted = false,
                
                AddedDate = dateAdded,
                LastUpdatedDate = dateAdded,
            };
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");

            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    AddUpdateDate = dateAdded,
                    NormalizedName = "Admin",
                },
                new Role
                {
                    Id = 2,
                    Name = "Member",
                    AddUpdateDate = dateAdded,
                    NormalizedName = "Member",
                }
                );
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                });

        }

        private void InitializeAuthorTypeData(ModelBuilder modelBuilder, DateTime dateAdded)
        {
            modelBuilder.Entity<AuthorType>().HasData(
                new AuthorType
                {
                    Id = 1,
                    TypeName = "Author",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 2,
                    TypeName = "Editor",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 3,
                    TypeName = "Translator",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 4,
                    TypeName = "Contributor",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 5,
                    TypeName = "Illustrator",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 6,
                    TypeName = "Co-Author",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 7,
                    TypeName = "Researcher",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new AuthorType
                {
                    Id = 8,
                    TypeName = "Other / Not Specified",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                });
        }

        private void InitializeAuthorData(ModelBuilder modelBuilder, DateTime dateAdded)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    FirstName = "Mark",
                    LastName = "Twain",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new Author
                {
                    Id = 2,
                    FirstName = "Charles",
                    LastName = "Dickens",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 3,
                    FirstName = "Agatha",
                    LastName = "Christie",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 4,
                    FirstName = "William",
                    LastName = "Shakespeare",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 5,
                    FirstName = "Ian",
                    LastName = "Fleming",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 6,
                    FirstName = "Daniel",
                    LastName = "Defoe",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 7,
                    FirstName = "Jonathan",
                    LastName = "Swift",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 8,
                    FirstName = "Lewis",
                    LastName = "Carroll",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 9,
                    FirstName = "J.R.R",
                    LastName = "Tolkien",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 10,
                    FirstName = "Alexandre",
                    LastName = "Dumas",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 11,
                    FirstName = "Jules",
                    LastName = "Verne",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 12,
                    FirstName = "Leo",
                    LastName = "Tolstoy",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 13,
                    FirstName = "Fyodor",
                    LastName = "Dostoevsky",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 14,
                    FirstName = "Rabindranath",
                    LastName = "Tagore",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 15,
                    FirstName = "Arthur",
                    MiddleName = "Conan",
                    LastName = "Doyle",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 16,
                    FirstName = "George",
                    LastName = "Orwell",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 17,
                    FirstName = "Robert",
                    MiddleName = "Louis",
                    LastName = "Stevenson",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 18,
                    FirstName = "Rudyard",
                    LastName = "Kipling",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 19,
                    FirstName = "Oscar",
                    LastName = "Wilde",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 20,
                    FirstName = "H. G.",
                    LastName = "Wells",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 21,
                    FirstName = "Salman",
                    LastName = "Rushdie",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 22,
                    FirstName = "Joseph",
                    LastName = "Conrad",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Author
                {
                    Id = 23,
                    FirstName = "Ernest",
                    LastName = "Hemingway",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                });
        }

        private void InitializePublisherData(ModelBuilder modelBuilder, DateTime dateAdded)
        {
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher
                {
                    Id = 1,
                    PublisherName = "HarperCollins Publishers LLC",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,
                }, new Publisher
                {
                    Id = 2,
                    PublisherName = "Penguin Random House",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Publisher
                {
                    Id = 3,
                    PublisherName = "Hachette Books",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Publisher
                {
                    Id = 4,
                    PublisherName = "Macmillan Publishers",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                }, new Publisher
                {
                    Id = 5,
                    PublisherName = "Simon & Schuster LLC",
                    Deleted = false,
                    AddedDate = dateAdded,
                    LastUpdatedDate = dateAdded,
                    AddedBy = Constants.SYSTEM_USER_ID,

                });
        }

        #endregion
    }
}
