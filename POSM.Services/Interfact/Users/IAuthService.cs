using POSM.Core.Enums;
using System.Threading.Tasks;

namespace POSM.Services.Interfact.Users
{
    public interface IAuthService
    {
        Task<dynamic> GetUserLogin(GrantType type, string email, string password, string refreshToken);
    }
}
