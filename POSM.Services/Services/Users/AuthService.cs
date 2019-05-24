using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POSM.Core.Constants;
using POSM.Core.Enums;
using POSM.Core.Helpers;
using POSM.Core.Models;
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
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _userDbSet;
        private readonly SignInManager<User> _signInManager;

        public AuthService(SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userDbSet = context.Set<User>();
            _signInManager = signInManager;
        }

        public async Task<dynamic> GetUserLogin(GrantType type, string username, string password, string refreshToken)
        {
            {
                if (type == GrantType.Password)
                {
                    // validate email
                    if (string.IsNullOrEmpty(username))
                        return "Email is required";

                    // standardlize email
                    username = username.Trim().ToLower();
                    username = username.Replace("'", "");
                    var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                    if (!result.Succeeded)
                        return "Invalid email or password";

                    // check if deleted
                    var userTypePassword = await _userDbSet.Where(x => x.Email == username || x.UserName == username).FirstOrDefaultAsync();
                    if (userTypePassword.IsDeleted)
                        return CommonConstant.USERHASBEENDELETED;

                    // skip case: RequiresTwoFactor, IsLockedOut
                    return userTypePassword;
                }

                var data = EncryptHelper.Decrypt<UserPayload>(refreshToken);
                if (data == null || data.UserId == null || !data.IsRefreshToken)
                    return CommonConstant.REFRESHTOKENISINVALID;
                if (data.ExpiredInUtc < DateTime.UtcNow)
                    return CommonConstant.REFRESHTOKENISINVALID;

                // check if deleted
                var user = await _userDbSet.Where(x => x.Id == data.UserId).FirstOrDefaultAsync();
                if (user.IsDeleted)
                    return CommonConstant.USERHASBEENDELETED;

                return user;
            }
        }

        public Task UpdateTheNewToken(int userIdInput, string tokenInput, bool isLoginWeb = true)
        {
            throw new NotImplementedException();
        }
    }
}
