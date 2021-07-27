using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [StringLength(maximumLength:35,ErrorMessage = "uzunluq 35-den böyük ola bilməz!!!")]
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}
