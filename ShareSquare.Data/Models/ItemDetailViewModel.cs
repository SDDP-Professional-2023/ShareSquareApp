using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class ItemDetailViewModel
    {
        public Item Item { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
