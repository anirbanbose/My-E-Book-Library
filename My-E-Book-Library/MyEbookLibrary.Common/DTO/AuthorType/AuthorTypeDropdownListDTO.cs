using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.AuthorType
{
    public class AuthorTypeDropdownListDTO
    {
        public int Id { get; }
        public string TypeName { get; }
        public AuthorTypeDropdownListDTO(int id, string typeName)
        {
            Id = id;
            TypeName = typeName;
        }
    }
}
