using System.ComponentModel.DataAnnotations;

namespace GroanZone.ViewModels
{
    public class RegisterVM
    {
        [Required, MinLength(2)]
        public string Username { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(8), DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; } = "";
    }

    public class LoginVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}