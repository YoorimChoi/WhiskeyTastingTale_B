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
        private readonly WhiskeyContext _whiskeyContext;

        public ReviewRepository(ReviewContext reviewContext, UserContext userContext, WhiskeyContext whiskeyContext)
        {
            _reviewContext = reviewContext;
            _userContext = userContext;
            _whiskeyContext = whiskeyContext;
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

        internal async Task<ReviewUserPageDTO> GetByWhiskeyId(int whiskey_id, int page = 1, int pageSize = 4)
        {
            var reviews = await _reviewContext.reviews
                .Where(review => review.whiskey_id == whiskey_id)
                .Skip((page-1)* pageSize).Take(pageSize)
                .ToListAsync();

            var totalCount = await _reviewContext.reviews
                .Where(review => review.whiskey_id == whiskey_id).CountAsync(); 

            var userIDs = reviews.Select(r => r.user_id).Distinct().ToList();
            var users = await _userContext.users
                .Where(user => userIDs.Contains(user.user_id))
                .ToDictionaryAsync(user => user.user_id, user => user.nickname); // 사용자 ID를 키로 사용하여 닉네임을 사전에 저장


            var reviewUsers = reviews.Select(review => {
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

            var result = new ReviewUserPageDTO
            {
                reviews = reviewUsers,
                page = page,
                totalCount = totalCount,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount /pageSize)
            }; 

            return result; 
        }
        internal async Task<ReviewWhiskeyPageDTO> GetByUserId(int user_id, int page =1, int pageSize=4)
        {
            var reviews = await _reviewContext.reviews.Where(x => x.user_id == user_id)
                .Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            var totalCount = await _reviewContext.reviews.Where(x => x.user_id == user_id).CountAsync(); 

            var whiskeyIDs = reviews.Select(r => r.whiskey_id).Distinct().ToList();
            var whiskeys = await _whiskeyContext.whiskeys.Where(whiskey => whiskeyIDs.Contains(whiskey.whiskey_id))
                .ToDictionaryAsync(whiskey => whiskey.whiskey_id, whiskey => whiskey);

            var reviewWhiskey = reviews.Select(review =>
            {
                Whiskey whiskey = whiskeys.TryGetValue(review.whiskey_id, out Whiskey? value) ? value : null;
                return new ReviewWhiskeyDTO
                {
                    review_id = review.review_id,
                    user_id = review.user_id,
                    whiskey_id = review.whiskey_id,
                    whiskey_name = whiskey.whiskey_name,
                    whiskey_img_index = whiskey.img_index,
                    rating = review.rating,
                    review_text = review.review_text,
                    created_date = review.created_date,
                    updated_date = review.updated_date
                };
            }).ToList();

            var result = new ReviewWhiskeyPageDTO
            {
                reviews = reviewWhiskey,
                page = page,
                totalCount = totalCount,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return result; 
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

        internal async Task<ReviewUserWhiskeyPageDTO> GetAll(int page, int pageSize = 10)
        {
            var reviews = await _reviewContext.reviews
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var totalCount = await _reviewContext.reviews.CountAsync(); 

            var userList = reviews.Select(x=> x.user_id).Distinct().ToList();
            var whiskeyList =reviews.Select(x=>x.whiskey_id).Distinct().ToList();

            var users = await _userContext.users
                .Where(x => userList.Contains(x.user_id))
                .ToDictionaryAsync(x => x.user_id, x=>x); 
            var whiskeys = await _whiskeyContext.whiskeys
                .Where(x => whiskeyList.Contains(x.whiskey_id))
                .ToDictionaryAsync(x => x.whiskey_id, x => x);

            var reviewUserWhiskeys = reviews.Select(review => {
                Whiskey whiskey = whiskeys.TryGetValue(review.whiskey_id, out Whiskey? value_whiskey) ? value_whiskey : null;
                User user = users.TryGetValue(review.user_id, out User? value_user) ? value_user : null;

                return new ReviewUserWhiskeyDTO()
                {
                    review_id = review.review_id,
                    user_id = review.user_id,
                    user_nickname = user.nickname, 
                    user_email = user.email,
                    whiskey_id = review.whiskey_id,
                    whiskey_name = whiskey.whiskey_name,
                    img_index = whiskey.img_index,
                    rating = review.rating,
                    review_text = review.review_text,
                    created_date = review.created_date,
                    updated_date = review.updated_date
                };  
            }).ToList();

            var result = new ReviewUserWhiskeyPageDTO()
            {
                reviews = reviewUserWhiskeys,
                page = page,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return result;
        }

        internal async Task<ReviewUserWhiskeyPageDTO> GetByNickname(string nickname, int page, int pageSize = 10)
        {
            var users = await _userContext.users
                .Where(x => x.nickname.Contains(nickname))
                .ToDictionaryAsync(x => x.user_id, x => x);
            var userIDList = users.Select(x => x.Key).Distinct().ToList();

            var reviews = await _reviewContext.reviews
                .Where(x => userIDList.Contains(x.user_id)).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var totalCount = await _reviewContext.reviews.Where(x => userIDList.Contains(x.user_id)).CountAsync();

            var whiskeyIDList = reviews.Select(x => x.whiskey_id).ToList();
            var whiskeys = await _whiskeyContext.whiskeys
                .Where(x => whiskeyIDList.Contains(x.whiskey_id))
                .ToDictionaryAsync(x => x.whiskey_id, x => x);


            var reviewUserWhiskeys = reviews.Select(review =>
            {
                var user = users.TryGetValue(review.user_id, out User? value) ? value : null;
                var whiskey = whiskeys.TryGetValue(review.whiskey_id, out Whiskey? value_whiskey) ? value_whiskey : null;

                return new ReviewUserWhiskeyDTO()
                {
                    review_id = review.review_id,
                    user_id = user.user_id,
                    user_nickname = user.nickname,
                    user_email = user.email,
                    whiskey_id = whiskey.whiskey_id,
                    whiskey_name = whiskey.whiskey_name,
                    img_index = whiskey.img_index,
                    rating = review.rating,
                    review_text = review.review_text,
                    created_date = review.created_date,
                    updated_date = review.updated_date
                };
            }).ToList();

            return new ReviewUserWhiskeyPageDTO()
            {
                reviews = reviewUserWhiskeys, 
                page = page,
                pageSize = pageSize, 
                totalPages = (int)Math.Ceiling((double)totalCount/pageSize)
            }; 

        }

        internal async Task<ReviewUserWhiskeyPageDTO> GetByWhiskey(string searchString, int page, int pageSize = 10)
        {
            var whiskeys = await _whiskeyContext.whiskeys
                .Where(x => x.whiskey_name.Contains(searchString))
                .ToDictionaryAsync(x => x.whiskey_id, x => x);
            var whiskeyIDList = whiskeys.Select(x => x.Key).Distinct().ToList();

            var reviews = await _reviewContext.reviews
                .Where(x => whiskeyIDList.Contains(x.whiskey_id)).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var totalCount = await _reviewContext.reviews.Where(x => whiskeyIDList.Contains(x.whiskey_id)).CountAsync();

            var UserIDList = reviews.Select(x => x.user_id).ToList();
            var users = await _userContext.users
                .Where(x => UserIDList.Contains(x.user_id))
                .ToDictionaryAsync(x => x.user_id, x => x);


            var reviewUserWhiskeys = reviews.Select(review =>
            {
                var user = users.TryGetValue(review.user_id, out User? value) ? value : null;
                var whiskey = whiskeys.TryGetValue(review.whiskey_id, out Whiskey? value_whiskey) ? value_whiskey : null;

                return new ReviewUserWhiskeyDTO()
                {
                    review_id = review.review_id,
                    user_id = user.user_id,
                    user_nickname = user.nickname,
                    user_email = user.email,
                    whiskey_id = whiskey.whiskey_id,
                    whiskey_name = whiskey.whiskey_name,
                    img_index = whiskey.img_index,
                    rating = review.rating,
                    review_text = review.review_text,
                    created_date = review.created_date,
                    updated_date = review.updated_date
                };
            }).ToList();

            return new ReviewUserWhiskeyPageDTO()
            {
                reviews = reviewUserWhiskeys,
                page = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

        }
    }
}