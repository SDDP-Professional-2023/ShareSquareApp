using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models.Domain
{
    public class Error
    {
        [Key]
        public int ErrorId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Text { get; set; } // A description of the error

        [Required]
        [StringLength(100)]
        public string Type { get; set; } // The type of error

        public DateTime Timestamp { get; set; } // The time the error occurred

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // The status of the error (e.g., "unresolved", "resolved")

        [StringLength(500)]
        public string Source { get; set; } // The location (e.g., class, method) where the error occurred

        [StringLength(2000)]
        public string StackTrace { get; set; } // The call stack at the time the error occurred

        public string Data { get; set; } // Any additional non-sensitive data that might be helpful in debugging the error
    }
}
