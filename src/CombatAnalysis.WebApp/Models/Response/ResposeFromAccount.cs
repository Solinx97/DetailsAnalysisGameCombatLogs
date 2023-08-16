using CombatAnalysis.WebApp.Models.User;

namespace CombatAnalysis.WebApp.Models.Response
{
    public struct ResponseFromAccount
    {
        public AppUserModel User { get; set; }

        public string RefreshToken { get; set; }
    }
}
