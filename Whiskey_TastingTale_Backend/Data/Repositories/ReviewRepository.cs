using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class ReviewRepository
    {
        private readonly ReviewContext _reviewContext;
        private readonly UserContext _userContext;

        public ReviewRepository(ReviewContext reviewContext, UserContext userContext)
        {
            _reviewContext = reviewContext;
            _userContext = userContext;
        }

        internal async Task<Review> Create(Review review)
        {
            var result = _reviewContext.reviews.Add(review);
            await _reviewContext.SaveChangesAsync();

            return result.Entity;
        }
        internal async Task<Review> Delete(long review_id)
        {
            var review = await _reviewContext.reviews.FindAsync(review_id);
            _reviewContext.reviews.Remove(review);
            await _reviewContext.SaveChangesAsync();

            return review;
        }

        internal async Task<List<ReviewUserDTO>> GetByWhiskeyId(int whiskey_id)
        {
            var reviews = await _reviewContext.reviews
                .Where(review => review.whiskey_id == whiskey_id)
                .ToListAsync();

            var userIDs = reviews.Select(r => r.user_id).Distinct().ToList();
            var users = await _userContext.users
                .Where(user => userIDs.Contains(user.user_id))
                .ToDictionaryAsync(user => user.user_id, user => user.nickname); // 사용자 ID를 키로 사용하여 닉네임을 사전에 저장


            var result = reviews.Select(review => {
                string nickname = users.TryGetValue(review.user_id, out string? value) ? value : "Unknown User";
                return new ReviewUserDTO
                {
                    review_id = review.review_id,
                    whiskey_id = review.whiskey_id,
                    user_id = review.user_id,
                    rating = review.rating,
                    review_text = review.review_text,
                    created_date = review.created_date,
                    updated_date = review.updated_date,
                    user_nickname = nickname 
                };
            }).ToList();

            return result; 
        }
        internal async Task<List<Review>> GetByUserId(int user_id)
        {
            return await _reviewContext.reviews.Where(x => x.user_id == user_id).ToListAsync();
        }

        internal async Task<Review> Update(Review review)
        {
            var origin = await _reviewContext.reviews.FindAsync(review.review_id);
            origin.user_id = review.user_id;
            origin.whiskey_id = review.whiskey_id;
            origin.review_text = review.review_text;
            origin.rating = review.rating;


            var result = _reviewContext.reviews.Update(origin);
            await _reviewContext.SaveChangesAsync();

            return result.Entity;

        }
    }
}