using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;

namespace ShareSquare.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IItemService _itemService;
        private readonly IErrorService _errorService;

        public ReviewController(IReviewService reviewService, UserManager<IdentityUser> userManager, IItemService itemService, IErrorService errorService)
        {
            _reviewService = reviewService;
            _userManager = userManager;
            _itemService = itemService;
            _errorService = errorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(ReviewDTO reviewDTO)
        {
            try { 
                if (ModelState.IsValid)
                {
                    var reviewerUser = (ApplicationUser)await _userManager.GetUserAsync(User);
                    var item = await _itemService.GetItemById(reviewDTO.ItemId);
                    var reviewedUser = item?.User;

                    var review = _reviewService.AddReview(reviewDTO, reviewedUser?.Id, reviewerUser);

                    var userReviews = await _reviewService.GetReviewsByID(reviewedUser.Id);

                    var totalRating = userReviews.Sum(r => r.Rating) + review.Rating;
                    reviewedUser.Rating = totalRating / (userReviews.Count + 1);

                    var result = await _userManager.UpdateAsync(reviewedUser);

                    return RedirectToAction("Details", "Item", new { id = reviewDTO.ItemId });
                }

                return View(reviewDTO);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }
    }
}
