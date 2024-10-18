using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Context;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WhiskeyRequestRepository
    {
        private readonly WhiskeyRequestContext _requestContext;
        public WhiskeyRequestRepository(WhiskeyRequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        internal async Task<WhiskeyRequest> AddWhiskeyRequest(WhiskeyRequest request)
        {
            var result = await _requestContext.whiskeyRequests.AddAsync(request);
            await _requestContext.SaveChangesAsync();

            return result.Entity; 
        }

        internal async Task<WhiskeyRequest> DeleteWhiskeyReqeust(int request_id, int user_id, string role)
        {
            var origin = await _requestContext.whiskeyRequests.FindAsync(request_id);

            if (role == "user" && user_id != origin.user_id) return null; 

            var result = _requestContext.whiskeyRequests.Remove(origin);
            await _requestContext.SaveChangesAsync();

            return result.Entity;
        }

        internal async Task<List<WhiskeyRequest>> GetAllRequest()
        { 
            return await _requestContext.whiskeyRequests.AsQueryable().ToListAsync();
        }

        internal async Task<List<WhiskeyRequest>> GetByUserId(int user_id)
        {
            return await _requestContext.whiskeyRequests.Where(x=>x.user_id == user_id).ToListAsync();
        }

        internal async Task<WhiskeyRequest> UpdateWhiskeyReqeust(WhiskeyRequest request)
        {
            var temp = _requestContext.whiskeyRequests.Update(request);
            await _requestContext.SaveChangesAsync();

            return temp.Entity; 
        }
    }
}
