using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Data.Context;
using Whiskey_TastingTale_Backend.Data.Entities;

namespace Whiskey_TastingTale_Backend.Data.Repository
{
    public class UserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        internal async Task<User> AddUserAsync(User user)
        {
            var email = user.email;
            var nickname = user.nickname;

            var count = await _context.users.Where(x => x.email == email || x.nickname == nickname).CountAsync();
            if (count == 0)
            {
                var result = _context.users.Add(user);
                await _context.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        internal async Task<bool> CheckDuplicationEmail(string email)
        {
            var count = await _context.users.Where(x => x.email.Equals(email)).CountAsync();
            if (count == 0) return true;
            else return false; 
        }

        internal async Task<bool> CheckDuplicationNickname(string nickname)
        {
            var count = await _context.users.Where(x => x.nickname.Equals(nickname)).CountAsync();
            if (count == 0) return true;
            else return false;
        }

        internal async Task<User> DeleteUserAsync(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user != null)
            {
                user.is_active = false;
                _context.users.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        internal async Task<List<User>> GetAllAsync()
        {
            return await _context.users.AsQueryable().ToListAsync();
        }

        internal async Task<User> GetByEmailAsync(string email)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.email == email);
        }

        internal async Task<string> GetSaltAsync(string email)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.email == email);
            if (user == null) return null;
            else return user.salt;
        }

        internal async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.email == email);
            if (user != null && user.password_hash == password) return user;

            return null;
        }

        internal async Task<User> UpdateUserAsync(User user)
        {
            var result = _context.users.Update(user);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}