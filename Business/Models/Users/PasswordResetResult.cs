using Business.Enums.Users;

namespace Business.Models.Users
{
    public class PasswordResetResult
    {
        public PasswordResetStage Stage { get; set; }

        public string Message { get; set; }

        public bool WasPasswordReset { get; set; }

        public PasswordResetResult(
            PasswordResetStage stage, string message, bool wasPasswordReset = false)
        {
            Stage = stage;
            Message = message;
            WasPasswordReset = false;
        }
    }
}
