using System.ComponentModel.DataAnnotations;

namespace Business.Models.Users
{
    public class LoginAttempt
    {
        // TODO: Create error messages
        // TODO: Add prop validation attributes
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
