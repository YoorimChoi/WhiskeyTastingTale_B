using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Backend.Repository.Context;

namespace Whiskey_TastingTale_Backend.Repository
{
    public class ReviewRepository
    {
        private readonly ReviewContext _context;

        public ReviewRepository(ReviewContext context)
        {
            _context = context;
        }

        internal async Task<Review> Create(Review review)
        {
            var result = _context.reviews.Add(review); 
            await _context.SaveChangesAsync();

            return result.Entity; 
        }
        internal async Task<Review> Delete(long review_id)
        {
            var review = await _context.reviews.FindAsync(review_id);
            _context.reviews.Remove(review); 
            await _context.SaveChangesAsync();

            return review;
        }

        internal async Task<List<Review>> GetById(int whiskey_id)
        {
            return await _context.reviews.Where(x => x.whiskey_id == whiskey_id).ToListAsync(); 
        }

        internal async Task<Review> Update(Review review)
        {
            var origin = await _context.reviews.FindAsync(review.review_id);
            origin.user_id = review.user_id; 
            origin.whiskey_id = review.whiskey_id;
            origin.review_text = review.review_text;
            origin.rating = review.rating; 


            var result = _context.reviews.Update(origin);
            await _context.SaveChangesAsync();

            return result.Entity; 
            
        }
    }
}