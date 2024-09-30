using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Context;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WhiskeyRepository
    {
        private readonly WhiskeyContext _context;
        public WhiskeyRepository(WhiskeyContext context)
        {
            _context = context;
        }

        internal async Task<Whiskey> AddWhiskey(Whiskey whiskey)
        {
            var temp = new Whiskey()
            {
                alcohol_degree = whiskey.alcohol_degree,
                details = whiskey.details,
                maker = whiskey.maker
                //TODO) 이미지는 어떻게 넣지? img_index
            };
            await _context.whiskeys.AddAsync(temp);
            await _context.SaveChangesAsync();

            return temp;
        }

        internal async Task<Whiskey> DeleteWhiskey(int id)
        {
            var temp = _context.whiskeys.FindAsync(id).Result;
            if(temp != null)
            {
                _context.whiskeys.Remove(temp);
                await _context.SaveChangesAsync();
            }

            return temp;
        }

        internal async Task<List<Whiskey>> GetAllWhiskey()
        {
            return await _context.whiskeys.AsQueryable().ToListAsync();
        }

        internal async Task<Whiskey> GetById(int id)
        {
            return await _context.whiskeys.FindAsync(id);
        }


        internal async Task<Whiskey> UpdateWhiskey(Whiskey whiskey)
        {
            var temp = _context.whiskeys.Update(whiskey);
            await _context.SaveChangesAsync();

            return temp.Entity;
        }


    }
}
