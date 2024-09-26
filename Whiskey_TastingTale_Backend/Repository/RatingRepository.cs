using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Model;
using Whiskey_TastingTale_Backend.Repository.Context;

namespace Whiskey_TastingTale_Backend.Repository
{
    public class RatingRepository
    {
        private readonly RatingContext _context;

        public RatingRepository(RatingContext context)
        {
            _context = context;
        }

        internal async Task<Rating> Get(int whiskey_id)
        {
            var result = await _context.ratings.Where(x => x.whiskey_id == whiskey_id).FirstOrDefaultAsync();

            return result;
        }
    }
}