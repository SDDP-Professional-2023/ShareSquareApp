using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models.Domain;
using ShareSquare.Data.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.DOA
{
    public class ReviewDOA : IReviewDOA
    {
        private readonly ShareSquareDbContext _context;

        public ReviewDOA(ShareSquareDbContext context)
        {
            _context = context;
        }
        public void Add(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public async Task<List<Review>> GetReviewsByUserId(string userId)
        {
            return await _context.Reviews
                .Where(r => r.ReviewedUserId == userId)
                .ToListAsync();
        }

        public async Task<List<Review>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<int> GetReviewCount()
        {
            return await _context.Reviews.CountAsync();
        }
    }
}
