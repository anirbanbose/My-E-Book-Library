

namespace MyEbookLibrary.Common.DTO.Genre
{
    public class GenreListDTO
    {
        public int Id { get; }
        public string GenreName { get; }
        public int BookCount { get; }
        public int AddedBy { get; }

        public GenreListDTO(int id, string genreName, int bookCount, int addedBy) 
        { 
            Id = id;
            GenreName = genreName;
            BookCount = bookCount;
            AddedBy = addedBy;
        }
    }
}
