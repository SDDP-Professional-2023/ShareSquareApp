using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services.IServices
{
    public interface IReviewService
    {
        Review AddReview(ReviewDTO reviewDto, string reviewedUserId, ApplicationUser reviewerUser);
        Task<List<Review>> GetReviewsByID(string id);
    }
}
