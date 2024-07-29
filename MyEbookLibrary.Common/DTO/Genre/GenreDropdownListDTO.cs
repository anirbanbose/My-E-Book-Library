

namespace MyEbookLibrary.Common.DTO.Genre
{
    public class GenreDropdownListDTO 
    {
        public int Id { get; }
        public string GenreName { get; }
        public GenreDropdownListDTO(int id, string genreName)
        {
            Id = id;
            GenreName = genreName;
        }
    }
}
