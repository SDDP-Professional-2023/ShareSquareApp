using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class ReviewDTO
    {
        public int ItemId { get; set; }
        public decimal Rating { get; set; }

        [StringLength(1000)]
        public string Text { get; set; }

    }
}
