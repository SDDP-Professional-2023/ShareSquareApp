using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;

namespace ShareSquare.Controllers
{
    [Authorize]
    public class UserItemsController : Controller
    {
        private readonly IUserItemService _userItemService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IErrorService _errorService;

        public UserItemsController(IUserItemService userItemService, UserManager<IdentityUser> userManager, IErrorService errorService)
        {
            _userItemService = userItemService;
            _userManager = userManager;
            _errorService = errorService;
        }

        public async Task<IActionResult> Index(ItemStatus? status)
        {
            try { 
                var userId = _userManager.GetUserId(User);
                var items = await _userItemService.GetUserItemsAsync(userId, status);

                return View(items);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        public async Task<IActionResult> SoftDelete(int id)
        {
            try { 
                await _userItemService.SoftDeleteItemAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        public async Task<IActionResult> UpdateStatus(int id, ItemStatus status)
        {
            try { 
                await _userItemService.UpdateItemStatusAsync(id, status);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }
    }

}
