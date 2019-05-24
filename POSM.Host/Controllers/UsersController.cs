using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using POSM.Core.Constants;
using POSM.Core.Helpers;
using POSM.Core.Models;
using POSM.Domain.User;
using POSM.Host.Helpers;
using POSM.Host.Models.Users;
using POSM.Services.Interfact.Users;

namespace POSM.Host.Controllers
{
    [Route("api/[controller]")]
    [Authorize("DefaultPolicy")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService,
            IAuthService authService,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userService = userService;
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Test API

        //[HttpGet("test")]
        //[AllowAnonymous]
        //public async Task<IActionResult> TestLive()
        //{
        //    var testEmail = "vinh.tran@newoceaninfosys.com";
        //    var testUser = await _userService.FindUserByEmail(testEmail);
        //    if (testUser != null)
        //        return ApiHelper.Success(CommonConstant.SUCCESS, testUser.Id);

        //    testUser = new User
        //    {
        //        Email = testEmail,
        //        UserName = testEmail,
        //        FirstName = "Vinh",
        //        LastName = "Tran",
        //    };

        //    var result = await _userManager.CreateAsync(testUser, "123123");

        //    // check result
        //    if (result.Succeeded)
        //    {
        //        return ApiHelper.Success(CommonConstant.SUCCESS, testUser.Id);
        //    }

        //    return ApiHelper.BadRequest(ModelState);
        //}

        //[HttpGet("authtest")]
        //public async Task<IActionResult> TestAuth()
        //{
        //    var currentUserId = Convert.ToString(Request.Headers[CommonConstant.USERID]);
        //    var currentUser = await _userService.FindUserByUserId(Guid.Parse(currentUserId));

        //    if (currentUser != null)
        //        return ApiHelper.Success(CommonConstant.SUCCESS, currentUserId);

        //    return ApiHelper.BadRequest("Invalid user");
        //}

        //[HttpGet("relationtest")]
        //[AllowAnonymous]
        //public async Task<IActionResult> TestRelationship()
        //{
        //    var testEmail = "vinh@gmail.com";
        //    var testUserSupervee = new User() {
        //        Email = testEmail,
        //        UserName = testEmail,
        //        FirstName = "test",
        //        LastName = "supervee",
        //    };
        //    await _userService.InsertAsync(testUserSupervee);

        //    var supervisor = await _userManager.FindByEmailAsync("vinh.tran@newoceaninfosys.com");

        //    testUserSupervee.Supervior = supervisor;

        //    await _userService.UpdateAsync(testUserSupervee);

        //    supervisor = await _userManager.FindByEmailAsync("vinh.tran@newoceaninfosys.com");

        //    return ApiHelper.Success();
        //}

        #endregion

        #region Auth API

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Signin([FromBody] SignInRequest model)
        {
            //check ModelState
            if (!ModelState.IsValid)
                return ApiHelper.BadRequest(ModelState);

            var getUserResult =
                await _authService.GetUserLogin(model.Type, model.Email, model.Password, model.RefreshToken);
            if (getUserResult is string)
                return ApiHelper.BadRequest((string)getUserResult);

            var user = (User)getUserResult;
            var jsonResponse = new LoginResponse
            {
                AccessToken = EncryptHelper.Encrypt(new UserPayload
                {
                    UserId = user.Id,
                    ExpiredInUtc = DateTime.UtcNow.AddMinutes(CommonConstant.ACCESSTOKENMINUTEEXPIRED),
                    IsRefreshToken = false
                }),
                RefreshToken = string.IsNullOrEmpty(model.RefreshToken)
                    ? EncryptHelper.Encrypt(new UserPayload
                    {
                        UserId = user.Id,
                        ExpiredInUtc = DateTime.UtcNow.AddDays(CommonConstant.REFRESHTOKENDAYEXPIRED),
                        IsRefreshToken = true
                    })
                    : model.RefreshToken,
                TokenType = CommonConstant.TOKENSCHEMA,
                ExpiresIn = CommonConstant.ACCESSTOKENMINUTEEXPIRED * 60,
                //Role = user.Role.ToString()
            };

            return ApiHelper.Success(CommonConstant.SUCCESS, jsonResponse);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            var currentUserId = Request.Headers[CommonConstant.USERID];
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            //check ModelState
            if (!ModelState.IsValid)
                return ApiHelper.BadRequest(ModelState);

            // check change password
            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
                return ApiHelper.Success(CommonConstant.SUCCESS);

            // return error
            return ApiHelper.BadRequest(ModelState);
        }

        #endregion

        [HttpGet("{userid}")]
        public async Task<IActionResult> Get(string userid)
        {
            var currentUser = await _userService.FindUserByUserId(Guid.Parse(userid));

            // check result
            if (currentUser != null)
            {
                return ApiHelper.Success(CommonConstant.SUCCESS, currentUser);
            }

            return ApiHelper.BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserRequest model)
        {
            var currentUserId = Convert.ToString(Request.Headers[CommonConstant.USERID]);
            var currentUser = await _userService.FindUserByUserId(Guid.Parse(currentUserId));

            // init user           
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email.Replace("'", ""),
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            // check result
            if (result.Succeeded)
            {
                return ApiHelper.Success(CommonConstant.SUCCESS, user.Id);
            }

            return ApiHelper.BadRequest(ModelState);
        }
    }
}