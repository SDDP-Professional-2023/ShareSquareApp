using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services.IServices;

namespace ShareSquare.Controllers
{
    public class AdminController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IUserService _userService;
        private readonly IErrorService _errorService;
        public AdminController(IUserService userService, IItemService itemService, IErrorService errorService)
        {
            _userService = userService;
            _itemService = itemService;
            _errorService = errorService;   

        }

        // List of Users 
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return View(users);
        }

        // List of Errors
        public async Task<IActionResult> GetErrors()
        {
            var errors = await _errorService.GetAllErrorsAsync();
            return View(errors);
        }

        // List of Item
        public async Task<IActionResult> GetItems()
        {
            var items = await _itemService.GetItems();
            return View(items);
        }

        // List of Messages 
        //public async Task<IActionResult> GetMessages()
        //{

        //}

        //// List of Review
        //public async Task<IActionResult> GetReview()
        //{

        //}
    }
}
