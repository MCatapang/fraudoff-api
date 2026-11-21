using Business.Constants.ValidationMessages.Users;
using Business.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Users
{
    public class AuthChallengeInput
    {
        [Required(ErrorMessage = UserValidationMessage.RequiredChallengeName)]
        public string ChallengeName { get; set; }

        [Required(ErrorMessage = UserValidationMessage.RequiredChallengeResponses)]
        public Dictionary<string, string> ChallengeResponses { get; set; }

        [Required(ErrorMessage = UserValidationMessage.RequiredSession)]
        public string Session { get; set; }
    }
}
