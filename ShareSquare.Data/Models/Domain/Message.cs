using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models.Domain
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
        public bool Deleted { get; set; } = false;

        public virtual ApplicationUser Sender { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
    }
}
