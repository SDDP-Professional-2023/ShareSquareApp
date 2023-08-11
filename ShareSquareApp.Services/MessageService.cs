using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquare.Data.Models.Repository;
using ShareSquareApp.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageDOA _messageDOA;

        public MessageService(IMessageDOA messageDOA)
        {
            _messageDOA = messageDOA;
        }

        public async Task<Message> AddMessageAsync(ApplicationUser sender, ApplicationUser receiver, string content)
        {
            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Text = content,
                Timestamp = DateTime.UtcNow,
            };

            await _messageDOA.AddMessage(message);

            return message;
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            await _messageDOA.DeleteMessage(messageId);
        }

        public async Task<List<Message>> GetMessagesAsync(string userId)
        {
            return await _messageDOA.GetMessages(userId);
        }

        public async Task<List<Message>> GetMessagesAsync(string senderId, string receiverId)
        {
            return await _messageDOA.GetMessages(senderId, receiverId);
        }

        public async Task<List<MessageViewModel>> GetHistoricalMessagesAsync(string sender, string receiver)
        {
            var list = await _messageDOA.GetMessages(sender, receiver);
            var messages = list
                .Select(m => new MessageViewModel
                {
                    Id = m.MessageId,
                    SenderUsername = m.Sender.UserName,
                    ReceiverUsername = m.Receiver.UserName,
                    Content = m.Text,
                    Timestamp = m.Timestamp,
                    Deleted = m.Deleted,
                })
                .OrderBy(m => m.Timestamp)
                .ToList();
            return messages;
        }

    }
}
