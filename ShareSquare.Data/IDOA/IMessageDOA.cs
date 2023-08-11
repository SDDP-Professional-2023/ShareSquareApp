using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.IDOA
{
    public interface IMessageDOA
    {
        Task AddMessage(Message message);
        Task DeleteMessage(int messageId);
        Task<List<Message>> GetMessages(string userId);
        Task<List<Message>> GetMessages(string senderId, string receiverId);
    }
}
