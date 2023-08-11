using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class EmailServiceOptions
    {
        public string FromEmail { get; set; }
        public string AuthenticateEmail { get; set; }
        public string AuthenticatePassword { get; set; }
    }
}
