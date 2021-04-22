using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(StarterPracticeDbContext dbContext) : base(dbContext)
        {
        }



        public async Task<User> GetUserByEmail(string email)
        {
            // FirstOrDefault -- checks if something exists if not returns null
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<bool> SaltExists(string salt)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Salt == salt);
            if (user != null)
            {
                return false;
            }

            return true;
        }
    }
}
