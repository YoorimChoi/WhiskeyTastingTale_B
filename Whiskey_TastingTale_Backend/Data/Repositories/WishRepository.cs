using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WishRepository
    {
        private readonly WishContext _wishContext;

        public WishRepository(WishContext wishContext)
        {
            _wishContext = wishContext;
        }

        internal async Task<Wish> Create(Wish wish)
        {
            var result = _wishContext.wishs.Add(wish);
            await _wishContext.SaveChangesAsync();

            return result.Entity;
        }
        internal async Task<Wish> Delete(int wish_id)
        {
            var origin = await _wishContext.wishs.Where(x => x.wish_id == wish_id).FirstOrDefaultAsync();
            if (origin != null)
            {
                _wishContext.wishs.Remove(origin);
                await _wishContext.SaveChangesAsync();

            }
            return origin;
        }

        internal async Task<Wish> Get(int user_id, int whiskey_id)
        {
            return await _wishContext.wishs.Where(x => x.user_id == user_id && x.whiskey_id == whiskey_id).FirstOrDefaultAsync();
        }

        internal async Task<List<Wish>> GetByUserId(int user_id)
        {
            return await _wishContext.wishs.Where(x => x.user_id == user_id).ToListAsync();
        }

    }
}