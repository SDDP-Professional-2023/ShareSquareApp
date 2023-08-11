using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquare.Data.Models.Repository;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;

namespace ShareSquare.Hubs
{
    public class MessageHub : Hub
    {
        private readonly ShareSquareDbContext _db;
        public MessageHub(ShareSquareDbContext db)
        {
            _db = db;
        }
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync("MessageReceived", user, message);
        }

        public async Task SendMessageToReceiver(string sender, string receiver, string message)
        {
            var receiverUser = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower());
            var senderUser = _db.Users.FirstOrDefault(u => u.Email.ToLower() == sender.ToLower());

            if (receiverUser != null && senderUser != null)
            {
                await Clients.User(receiverUser.Id).SendAsync("MessageReceived", sender, message);
                await Clients.User(receiverUser.Id).SendAsync("sendNotification", sender);
                await Clients.User(senderUser.Id).SendAsync("MessageReceived", sender, message);
            }

            
        }

    }

}
