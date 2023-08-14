using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.IDOA
{
    public interface IReviewDOA
    {
        void Add(Review review);
        Task<int> GetReviewCount();
        Task<List<Review>> GetReviews();
        Task<List<Review>> GetReviewsByUserId(string userId);
    }
}
