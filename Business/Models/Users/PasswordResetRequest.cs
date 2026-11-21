using System.ComponentModel.DataAnnotations;

namespace Business.Models.Users
{
    public class PasswordResetRequest
    {
        // TODO: Create error messages
        // TODO: Add prop validation attributes
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }
    }
}
