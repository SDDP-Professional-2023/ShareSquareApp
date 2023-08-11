using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models.Domain;
using ShareSquare.Data.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.DOA
{
    public class MessageDOA : IMessageDOA
    {
        private readonly ShareSquareDbContext _context;

        public MessageDOA(ShareSquareDbContext context)
        {
            _context = context;
        }

        public async Task AddMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessage(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.Deleted = true;
                _context.Entry(message).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Message>> GetMessages(string userId)
        {
            return await _context.Messages
                 .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.Sender.Id == userId || m.Receiver.Id == userId) && !m.Deleted)
                .OrderByDescending(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<List<Message>> GetMessages(string sender, string receiver)
        {
            return await _context.Messages
                .Where(m => (m.Sender.UserName == sender && m.Receiver.UserName == receiver) ||
                     (m.Sender.UserName == receiver && m.Receiver.UserName == sender) &&
                     !m.Deleted)
                .ToListAsync();
        }


    }
}
