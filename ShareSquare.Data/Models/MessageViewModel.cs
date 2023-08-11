using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Deleted { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }
    }
}
