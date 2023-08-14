using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class EntitiesCountViewModel
    {
        public int ReviewCount { get; set; }
        public int ErrorCount { get; set; }
        public int ItemCount { get; set; }
        public int UserCount { get; set; }
        public int MessageCount { get; set; }
    }
}
