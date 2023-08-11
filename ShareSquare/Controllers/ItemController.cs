using AutoMapper;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;

namespace ShareSquare.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IItemService _itemService;
        private readonly IFavoriteItemService _favoriteItemService;
        private readonly IReviewService _reviewService;
        private readonly IErrorService _errorService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public ItemController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IItemService itemService, IFavoriteItemService favoriteItemService, IReviewService reviewService, IErrorService errorService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _itemService = itemService;
            _favoriteItemService = favoriteItemService;
            _reviewService = reviewService;
            _errorService = errorService;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(FilterModel filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var items = await _itemService.GetItems(filter, user?.Id);

            return View(items.ToList());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try { 
                // Assuming you have a logged in user.
                var user = await _userManager.GetUserAsync(User);

                // Pass the user's ID to the view via ViewData.
                ViewData["UserId"] = user.Id;
                return View();
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //helps to prevent cross site request forgery
        public async Task<IActionResult> Create(ItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel);
            try { 
                ModelState.Remove("User");
                if (ModelState.IsValid)
                {

                    string uniqueFileName = null;
                    if (itemViewModel.ImageUrl != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        // using this class so that the same file name won't overide an existing file
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + itemViewModel.ImageUrl.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        itemViewModel.ImageUrl.CopyTo(new FileStream(filePath, FileMode.Create));
                    }

                    // Assuming you have a method to get the current user.
                    // This would depend on your specific authentication setup.
                    var user = (ApplicationUser) await _userManager.GetUserAsync(User);

                    // Assign the user to the item.
                    item.User = user;
                    item.ImageUrl = uniqueFileName;
                    var newItem = _itemService.CreateNewItem(item);

                    TempData["success"] = "category created successfully";
                    return RedirectToAction("Index", "Item");
                }

                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];

                    foreach (var error in modelStateVal.Errors)
                    {
                        _logger.LogError($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }

                return View(item);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try { 
                if (id == null)
                {
                    return NotFound();
                }
                var item = await _itemService.GetItemById(id);

                var userId = _userManager.GetUserId(User);
                var user = (ApplicationUser)await _userManager.GetUserAsync(User);
                var favoriteItem = await _favoriteItemService.GetFavoriteItems(userId);
                var isSaved = favoriteItem.Any(f => f.UserId == userId && f.ItemId == id);
                ViewData["IsSaved"] = isSaved;
                if (item == null)
                {
                    return NotFound();
                }

                var reviews = await _reviewService.GetReviewsByID(item.User.Id);

                var viewModel = new ItemDetailViewModel
                {
                    Item = item,
                    Reviews = reviews
                };

                // store the referrer URL in ViewData to use it in the view
                ViewData["Referrer"] = Request.Headers["Referer"].ToString();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try { 
                var item = await _itemService.GetItemById(id);
                if (item == null)
                {
                    return NotFound();
                }

                var imageUrl = item.ImageUrl;
                var imageToSee = imageUrl.Split("_")[1];
                item.ImageUrl = imageToSee;

                return View(item);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item model)
        {
            try { 
                ModelState.Remove("User");
                if (id != model.ItemId)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    await _itemService.UpdateItemAsync(model, model.Created_at);
                    return RedirectToAction(nameof(Index), "UserItems");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }
    }
}
