using Microsoft.EntityFrameworkCore;
using POSM.Data;
using POSM.Domain.User;
using POSM.Services.Interfact.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSM.Services.Services.Users
{
    public class UserService :  BaseService<User>, IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public Task<User> FindUserByUserId(Guid userid)
        {
            return _dbSet.Where(x => x.Id == userid).FirstOrDefaultAsync();
        }

        public Task<User> FindUserByUsername(string username)
        {
            return _dbSet.Where(x => x.UserName == username).FirstOrDefaultAsync();
        }

        public Task<User> FindUserByEmail(string email)
        {
            return _dbSet.Where(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}
