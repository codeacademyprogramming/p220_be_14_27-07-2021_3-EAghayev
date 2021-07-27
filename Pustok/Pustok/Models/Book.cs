using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        public int  PublisherId { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double CostPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double DiscountedPrice { get; set; }
        public int Rate { get; set; }

        [Required]
        [StringLength(maximumLength:20)]
        public string Code { get; set; }
        [StringLength(maximumLength: 1500)]
        public string Desc { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        [NotMapped]
        public IFormFile PosterImage { get; set; }

        [NotMapped]
        public IFormFile HoverPosterImage { get; set; }
        [NotMapped]
        public List<IFormFile> Images { get; set; }
        [NotMapped]
        public List<int> ImageIds { get; set; }

        public Genre Genre { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public List<BookImage> BookImages { get; set; }
        public List<BookTag> BookTags { get; set; }
    }
}
