using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using POSM.Core.Constants;
using POSM.Core.Helpers;
using POSM.Core.Models;
using System;
using System.Threading.Tasks;

namespace POSM.Host.Filter
{
    public class CheckHeaderTokenHandler : AuthorizationHandler<CheckHeaderToken>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckHeaderToken requirement)
        {
            //get mvcContext
            var mvcContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            if (mvcContext == null)
                throw new UnauthorizedAccessException("You not login yet");

            //get userId
            mvcContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues userToken);
            var data = EncryptHelper.Decrypt<UserPayload>(userToken);

            if (data == null || data.IsRefreshToken)
                throw new UnauthorizedAccessException("Invalid token");

            if (data.ExpiredInUtc < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Session timeout");

            mvcContext.HttpContext.Request.Headers[CommonConstant.USERID] = data.UserId.ToString();

            context.Succeed(requirement);
        }
    }

    public class CheckHeaderToken : IAuthorizationRequirement
    {
    }

}
