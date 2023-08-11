using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services.IServices
{
    public interface IMessageService
    {
        Task<Message> AddMessageAsync(ApplicationUser sender, ApplicationUser receiver, string content);
        Task DeleteMessageAsync(int messageId);
        Task<List<MessageViewModel>> GetHistoricalMessagesAsync(string sender, string receiver);
        Task<List<Message>> GetMessagesAsync(string userId);
        Task<List<Message>> GetMessagesAsync(string senderId, string receiverId);
    }
}
