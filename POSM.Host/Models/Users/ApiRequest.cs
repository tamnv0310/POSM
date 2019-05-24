using POSM.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace POSM.Host.Models.Users
{
    public class UserRequest
    {
        /// <summary>
        /// first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// sur name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// avatar url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress(ErrorMessage = "Email is invalid.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }
    }

    public class CreateUserRequest : UserRequest
    {

    }

    public class UpdateUserRequest : UserRequest
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

    }

    public class SignInRequest
    {
        /// <summary>
        /// grant type
        /// </summary>
        [Required(ErrorMessage = "Type is required")]
        [EnumDataType(typeof(GrantType), ErrorMessage = "Type must be 0 or 1")]
        public GrantType Type { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// refresh token
        /// </summary>
        public string RefreshToken { get; set; }
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public bool IsShowMenuAdmin { get; set; }
        public string Role { get; set; }
    }

    public class ChangePasswordRequest
    {
        /// <summary>
        /// old password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string OldPassword { get; set; }

        /// <summary>
        /// new password password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
}
