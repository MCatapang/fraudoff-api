using Amazon.CognitoIdentityProvider;

namespace Business.Models.Services.CognitoService
{
    public class AdditionalChallenge
    {
        public ChallengeNameType ChallengeName { get; set; }

        public Dictionary<string, string> ChallengeParameters { get; set; }

        public string Session { get; set; }

        public AdditionalChallenge(
            ChallengeNameType challengeName, 
            Dictionary<string, string> challengeParameters, 
            string session)
        {
            ChallengeName = challengeName;
            ChallengeParameters = challengeParameters;
            Session = session;
        }
    }
}
