using Business.Models.Users;
using Business.Services;
using Business.Services.Aws;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    [ApiController]
    // TODO: Create shared constant
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<LoginAuthResult> AuthenticateLogin(
            [FromServices] IUserAuthService userAuthService, [FromBody] LoginAttempt attempt)
        {
            return await userAuthService.AuthenticateLogin(attempt.UserName, attempt.Password);
        }

        [HttpPost("authenticate/challenge")]
        public async Task<AuthChallengeResult> RespondToAuthChallenge(
            [FromServices] IUserAuthService userAuthService, [FromBody] AuthChallengeInput input)
        {
            return await userAuthService.RespondToAuthChallenge(input);
        }

        [HttpPost("password/reset/request")]
        public async Task<PasswordResetResult> RequestPasswordReset(
            [FromServices] IUserAuthService userAuthService, [FromBody] PasswordResetRequest req)
        {
            return await userAuthService.RequestPasswordReset(req.UserName);
        }

        [HttpPost("password/reset/attempt")]
        public async Task<PasswordResetResult> AttemptPasswordReset(
            [FromServices] IUserAuthService userAuthService,
            [FromBody] PasswordResetAttempt attempt)
        {
            return await userAuthService.AttemptPasswordReset(
                attempt.ConfirmationCode, attempt.UserName, attempt.NewPassword);
        }
    }
}
