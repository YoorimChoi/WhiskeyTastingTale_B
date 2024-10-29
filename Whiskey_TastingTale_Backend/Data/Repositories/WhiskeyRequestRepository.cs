using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Entities;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.API.DTOs;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class WhiskeyRequestRepository
    {
        private readonly WhiskeyRequestContext _requestContext;
        private readonly UserContext _userContext;
        private readonly NotificationRepository _notiRepository; 
        public WhiskeyRequestRepository(WhiskeyRequestContext requestContext, UserContext userContext, NotificationRepository notiRepository)
        {
            _requestContext = requestContext;
            _userContext = userContext;
            _notiRepository = notiRepository;
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

        internal async Task<List<WhiskeyRequestUserDTO>> GetAllRequest()
        {
            var requests = await _requestContext.whiskeyRequests.AsQueryable().ToListAsync();

            var userList = requests.Select(x => x.user_id).Distinct().ToList();
            var users = await _userContext.users.Where(x => userList.Contains(x.user_id)).ToDictionaryAsync(x => x.user_id, x => x);

            var requestUsers = requests.Select(request =>
            {
                User user = users.TryGetValue(request.user_id, out User? value) ? value : null;
                return new WhiskeyRequestUserDTO
                {
                    user_id = user.user_id,
                    user_email = user.email, 
                    user_nickname = user.nickname,
                    request_id = request.request_id, 
                    img_index = request.img_index,
                    name = request.name,
                    alcohol_degree = request.alcohol_degree,
                    details = request.details,
                    maker = request.maker,
                    is_accepted = request.is_accepted,
                    is_completed = request.is_completed,
                    whiskey_id = request.whiskey_id 
                };
            }).ToList();

            return requestUsers;
        }

        internal async Task<List<WhiskeyRequest>> GetByUserId(int user_id)
        {
            return await _requestContext.whiskeyRequests.Where(x=>x.user_id == user_id).ToListAsync();
        }

        internal async Task<WhiskeyRequest> UpdateWhiskeyReqeust(WhiskeyRequest request)
        {
            var origin = await _requestContext.whiskeyRequests.FindAsync(request.request_id); 

            if(origin.is_completed == false && request.is_completed == true)
            {
                _notiRepository.SendResultOfRequest(request); 
            }

            origin.name = request.name;
            origin.details = request.details;
            origin.maker = request.maker;
            origin.details = request.details; 
            origin.is_accepted = request.is_accepted;
            origin.is_completed = request.is_completed;
            origin.img_index = request.img_index;
            origin.alcohol_degree = request.alcohol_degree; 
            origin.whiskey_id = request.whiskey_id; 

            var temp = _requestContext.whiskeyRequests.Update(origin);
            await _requestContext.SaveChangesAsync();

            return temp.Entity; 
        }
    }
}
