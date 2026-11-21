using Business.Models.Services.CognitoService;

namespace Business.Models.Users
{
    public class LoginAuthResult
    {
        public bool IsUserAuthenticated { get; set; }

        public string Message { get; set; }

        public AdditionalChallenge? AdditionalChallenge { get; set; }

        public LoginAuthResult(
            bool isUserAuthenticated, 
            string message, 
            AdditionalChallenge? additionalChallenge = null)
        {
            IsUserAuthenticated = isUserAuthenticated;
            Message = message;
            AdditionalChallenge = additionalChallenge;
        }
    }
}
