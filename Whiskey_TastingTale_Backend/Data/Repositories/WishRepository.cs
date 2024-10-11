using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.API.DTOs;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WishRepository
    {
        private readonly WishContext _wishContext;
        private readonly WhiskeyContext _whiskeyContext; 

        public WishRepository(WishContext wishContext, WhiskeyContext whiskeyContext)
        {
            _wishContext = wishContext;
            _whiskeyContext = whiskeyContext;
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

        internal async Task<List<WishWhiskeyDTO>> GetByUserId(int user_id)
        {
            var wishs = await _wishContext.wishs.Where(x => x.user_id == user_id).ToListAsync();
            var whiskeyIDs = wishs.Select(wish => wish.whiskey_id).Distinct().ToList();
            var whiskeys = await _whiskeyContext.whiskeys.Where(whiskey => whiskeyIDs.Contains(whiskey.whiskey_id))
                .ToDictionaryAsync(whiskey => whiskey.whiskey_id, whiskey=> whiskey);

            var result = wishs.Select(wish =>
            {
                var whiskey = whiskeys.TryGetValue(wish.whiskey_id, out Whiskey? value) ? value : null;
                return new WishWhiskeyDTO()
                {
                    wish_id = wish.whiskey_id,
                    user_id = wish.user_id, 
                    whiskey_id = wish.whiskey_id,
                    whiskey_name = whiskey.whiskey_name, 
                    img_index = whiskey.img_index,
                    alcohol_degree = whiskey.alcohol_degree, 
                    rating = whiskey.rating,
                    review_count = whiskey.review_count
                }; 
            }).ToList();

            return result; 
        }

    }
}