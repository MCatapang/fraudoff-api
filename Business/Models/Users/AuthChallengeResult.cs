using Business.Constants.ResultMessages;

namespace Business.Models.Users
{
    public class AuthChallengeResult
    {
        /// <summary>The name of the authentication challenge that the user attempted.</summary>
        public string AuthChallengeName { get; set; }

        /// <summary>Denotes whether the authentication challenge has been passed.</summary>
        public bool IsAuthChallengeSatisfied { get; set; }

        /// <summary>Contains more details regarding the authentication challenge result.</summary>
        public string Details { get; set; }

        public AuthChallengeResult(string authChallengeName)
        {
            AuthChallengeName = authChallengeName;
            IsAuthChallengeSatisfied = false;
            Details = ResultMessage.Error;
        }
    }
}
