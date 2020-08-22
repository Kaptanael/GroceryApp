using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryApp.Models;

namespace GroceryApp.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return _context as ApplicationDbContext; }
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var user = await ApplicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await ApplicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return user;
        }
    }
}
