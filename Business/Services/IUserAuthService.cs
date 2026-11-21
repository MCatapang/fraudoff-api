using Business.Models.Users;

namespace Business.Services
{
    public interface IUserAuthService
    {
        Task<LoginAuthResult> AuthenticateLogin(string userName, string password);

        Task<AuthChallengeResult> RespondToAuthChallenge(AuthChallengeInput input);

        Task<PasswordResetResult> RequestPasswordReset(string userName);

        Task<PasswordResetResult> AttemptPasswordReset(
            string confirmationCode, string userName, string newPassword);
    }
}
