using CombatAnalysis.WebApp.Models.User;

namespace CombatAnalysis.WebApp.Models.Response
{
    public struct ResponseFromAccount
    {
        public UserModel User { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
