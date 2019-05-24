using POSM.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSM.Services.Interfact.Users
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> FindUserByUserId(Guid userid);
        Task<User> FindUserByUsername(string username);
        Task<User> FindUserByEmail(string email);

    }
}
