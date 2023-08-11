using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;
using Message = ShareSquare.Data.Models.Domain.Message;

namespace ShareSquare.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IItemService _itemService;
        private readonly IErrorService _errorService;

        public MessageController(IMessageService messageService, UserManager<IdentityUser> userManager, IItemService itemService, IErrorService errorService)
        {
            _messageService = messageService;
            _userManager = userManager;
            _itemService = itemService;
            _errorService = errorService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string receiverId, string content, int? itemId)
        {
            try { 
                var sender = (ApplicationUser) await _userManager.GetUserAsync(User);
                var receiver = (ApplicationUser) await _userManager.FindByIdAsync(receiverId);
                var item = itemId == null ? null : await _itemService.GetItemById((int)itemId);

                var messageContent = content + $" ---------- (ItemName: {item.Title}, ItemPrice: {item.Price} )";

                var message = await _messageService.AddMessageAsync(sender, receiver, messageContent);

                return RedirectToAction("Details", "Item" ,new {id = itemId});
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] MessageViewModel messageVM)
        {
            try { 
                ModelState.Remove("ItemName");
                ModelState.Remove("ItemPrice");
                var sender = (ApplicationUser) await _userManager.FindByEmailAsync(messageVM.SenderUsername);
                var receiver = (ApplicationUser)await _userManager.FindByEmailAsync(messageVM.ReceiverUsername);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _messageService.AddMessageAsync(sender, receiver, messageVM.Content);
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try { 
                await _messageService.DeleteMessageAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        public async Task<IActionResult> Index()
        {
            try { 
                var user = await _userManager.GetUserAsync(User);
                var messages = await _messageService.GetMessagesAsync(user.Id);

                var conversations = messages
                    .GroupBy(m => m.Sender.Id == user.Id ? m.Receiver.UserName : m.Sender.UserName)
                    .Select(g => new MessageConversationViewModel
                    {
                        Receiver = g.Key,
                        Messages = g.Select(m => new MessageViewModel
                        {
                            Id = m.MessageId,
                            SenderUsername = m.Sender.UserName,
                            ReceiverUsername = m.Receiver.UserName,
                            Content = m.Text,
                            Timestamp = m.Timestamp,
                            Deleted = m.Deleted,
                        }).OrderByDescending(m => m.Timestamp).ToList()
                    }).ToList();


                return View(conversations);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoricalMessages(string sender, string receiver)
        {
            try { 
                var messages = await _messageService.GetHistoricalMessagesAsync(sender, receiver);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }
    }
}
