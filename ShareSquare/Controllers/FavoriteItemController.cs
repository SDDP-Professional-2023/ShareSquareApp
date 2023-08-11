using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSquare.Data.DOA;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;

namespace ShareSquare.Controllers
{
    [Authorize]
    public class FavoriteItemController : Controller
    {
        private readonly ILogger<FavoriteItemController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFavoriteItemService _favoriteItemService;
        private readonly IErrorService _errorService;

        public FavoriteItemController(ILogger<FavoriteItemController> logger, UserManager<IdentityUser> userManager, IFavoriteItemService favoriteItemService, IErrorService errorService)
        {
            _logger = logger;
            _userManager = userManager;
            _favoriteItemService = favoriteItemService;
            _errorService = errorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try { 
                var userId = _userManager.GetUserId(User);
                var favoriteItems = await _favoriteItemService.GetFavoriteItems(userId);
                return View(favoriteItems);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddToFavorites(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var favoriteItem = new FavoriteItem
            {
                UserId = user.Id,
                ItemId = id
            };

            _favoriteItemService.AddToFavorites(favoriteItem);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteItem(int id)
        {
            try { 
                await _favoriteItemService.RemoveFavoriteItem(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

    }
}
