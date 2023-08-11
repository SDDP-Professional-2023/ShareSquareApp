using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models.Domain
{
    public class Review : BaseEntity
    {
        [Key]
        public int ReviewId { get; set; }
        public string ReviewedUserId { get; set; }
        public virtual ApplicationUser ReviewerUser { get; set; }

        public decimal Rating { get; set; }

        [StringLength(1000)]
        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
