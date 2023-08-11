using Microsoft.AspNetCore.Http;
using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShareSquare.Data.Models
{
    public class ItemViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "Item type")]
        public string ItemType { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        [Display(Name = "Author")]
        public string AuthorOrDeveloper { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        [Display(Name = "ReleaseYear")]
        public int PublicationYearOrReleaseYear { get; set; }
        public IFormFile ImageUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string Condition { get; set; }

        [Required]
        [StringLength(20)]
        public string Language { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int Price { get; set; }
    }
}
