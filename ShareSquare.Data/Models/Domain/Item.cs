using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models.Domain
{
    public class Item : BaseEntity
    {
        [Key]
        public int ItemId { get; set; }

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
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string Condition { get; set; }

        [Required]
        [StringLength(20)]
        public string Language { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int Price { get; set; }

        public ItemStatus Status { get; set; } = ItemStatus.Active;

        public virtual ApplicationUser User { get; set; }
    }
}

public enum ItemStatus
{
    Active,
    Done,
    Archived,
    Deleted // for soft-deletes
}
