using AutoMapper;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewDOA _reviewDOA;
        private readonly IMapper _mapper;
        public ReviewService(IReviewDOA reviewDOA, IMapper mapper)
        {
            _reviewDOA = reviewDOA;
            _mapper = mapper;
        }
        public Review AddReview(ReviewDTO reviewDto, string reviewedUserId, ApplicationUser reviewerUser)
        {
            Review review = _mapper.Map<Review>(reviewDto);
            review.ReviewedUserId = reviewedUserId;
            review.ReviewerUser = reviewerUser;
            _reviewDOA.Add(review);

            return review;
        }

        public async Task<List<Review>> GetReviewsByID(string id)
        {
            var reviews = await _reviewDOA.GetReviewsByUserId(id);
            return reviews;
        }

        public async Task<List<Review>> GetReviews()
        {
            var reviews = await _reviewDOA.GetReviews();
            return reviews;
        }

        public async Task<int> GetReviewsCount() 
        {
            return await _reviewDOA.GetReviewCount();
        }
    }
}
