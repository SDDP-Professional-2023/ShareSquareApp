using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services.IServices;
using Message = ShareSquare.Data.Models.Domain.Message;

namespace ShareSquare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IUserService _userService;
        private readonly IErrorService _errorService;
        private readonly IReviewService _reviewService;
        private readonly IMessageService _messageService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AdminController(IUserService userService, IItemService itemService, IErrorService errorService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IReviewService reviewService, IMessageService messageService)
        {
            _userService = userService;
            _itemService = itemService;
            _errorService = errorService;
            _userManager = userManager;
            _signInManager = signInManager;
            _reviewService = reviewService;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviewCount = await _reviewService.GetReviewsCount();
            var errorCount = await _errorService.GetErrorsCount();
            var itemCount = await _itemService.GetItemsCount();
            var userCount = await _userService.GetUsersCount();
            var messageCount = await _messageService.GetMessagesCount();

            var viewModel = new EntitiesCountViewModel
            {
                ReviewCount = reviewCount,
                ErrorCount = errorCount,
                ItemCount = itemCount,
                UserCount = userCount,
                MessageCount = messageCount
            };

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            // checks the data submitted in the form complies with all the validation rules
            // deined in the model
            if (ModelState.IsValid)
            {
                // signs the user into the application 
                // we are going to have a remeberMe bool flag in our applicaition
                // if true and cookie has not expired we would log the user in the next time he want to user our 
                // application without entering his credentials 
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                      return RedirectToAction("Index", "Admin");
                }

                if (result.IsLockedOut)
                {
                    return View("Locked");
                }
                else
                {
                    // displan an errror message when login is not succesfull
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Admin");
        }

        // List of Users 
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            var applicationUsers = users.OfType<ApplicationUser>().ToList();
            var viewModel = new ViewModel<ApplicationUser> { Elements = applicationUsers };
            return View(viewModel);
        }

        // List of Errors
        public async Task<IActionResult> GetErrors()
        {
            var errors = await _errorService.GetAllErrorsAsync();
            var viewModel = new ViewModel<Error> { Elements = errors };
            return View(viewModel);
        }

        // List of Item
        public async Task<IActionResult> GetItems()
        {
            var items = await _itemService.GetAllItems();
            var viewModel = new ViewModel<Item> { Elements = items };
            return View(viewModel);
        }

        // List of Messages 
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _messageService.GetAllMessages();
            var viewModel = new ViewModel<Message> { Elements = messages };
            return View(viewModel);
        }

        // List of Review
        public async Task<IActionResult> GetReview()
        {
            var reviews = await _reviewService.GetReviews();
            foreach (var review in reviews)
            {
                var reviewedUser = await _userManager.FindByIdAsync(review.ReviewedUserId);
                var email = reviewedUser?.Email;
                review.ReviewedUserId = email;
            }
            var viewModel = new ViewModel<Review> { Elements = reviews };
            return View(viewModel);
        }
    }
}
