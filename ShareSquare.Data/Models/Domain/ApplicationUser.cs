using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        //public double Rating { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
