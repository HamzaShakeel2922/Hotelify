using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.DTOs
{
    public class LoginUserDTOs
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
