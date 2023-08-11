using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class FilterModel
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Language { get; set; }
        public string Condition { get; set; }
        public int Price { get; set; }
    }
}
