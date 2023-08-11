using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models
{
    public class MessageConversationViewModel
    {
        public string Receiver { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}
