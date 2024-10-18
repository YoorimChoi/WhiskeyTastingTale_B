using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.API.DTOs;
using Azure;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WhiskeyRepository
    {
        private readonly WhiskeyContext _whiskeyContext;
        private readonly WhiskeyRequestContext _requestContext;
        public WhiskeyRepository(WhiskeyContext whiskeyContext, WhiskeyRequestContext requestContext)
        {
            _whiskeyContext = whiskeyContext;
            _requestContext = requestContext;
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
            await _whiskeyContext.whiskeys.AddAsync(temp);
            await _whiskeyContext.SaveChangesAsync();

            return temp;
        }


        internal async Task<Whiskey> DeleteWhiskey(int id)
        {
            var temp = _whiskeyContext.whiskeys.FindAsync(id).Result;
            if(temp != null)
            {
                _whiskeyContext.whiskeys.Remove(temp);
                await _whiskeyContext.SaveChangesAsync();
            }

            return temp;
        }

        internal async Task<List<Whiskey>> GetAllWhiskey()
        {
            return await _whiskeyContext.whiskeys.AsQueryable().ToListAsync();
        }

        internal async Task<Whiskey> GetById(int id)
        {
            return await _whiskeyContext.whiskeys.FindAsync(id);
        }

        internal async Task<WhiskeyPageDTO> GetByName(string name, int page=1, int pageSize=9 )
        {
            await _whiskeyContext.whiskeys.Where(x => x.whiskey_name.Contains(name)).ToListAsync();

            var whiskeys = await _whiskeyContext.whiskeys.Where(x=>x.whiskey_name.Contains(name))
                                    .Skip((page-1)* pageSize).Take(pageSize).ToListAsync();

            var totalCount = await _whiskeyContext.whiskeys.Where(x => x.whiskey_name.Contains(name)).CountAsync();

            var result = new WhiskeyPageDTO
            {
                whiskeys = whiskeys,
                page = page,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }; 

            return result; 
        }

        internal async Task<Whiskey> UpdateWhiskey(Whiskey whiskey)
        {
            var temp = _whiskeyContext.whiskeys.Update(whiskey);
            await _whiskeyContext.SaveChangesAsync();

            return temp.Entity;
        }


    }
}
