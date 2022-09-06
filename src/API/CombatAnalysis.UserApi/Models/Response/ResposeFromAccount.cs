using CombatAnalysis.DAL.Entities.User;

namespace CombatAnalysis.UserApi.Models.Response
{
    public struct ResponseFromAccount
    {
        public ResponseFromAccount(User user, string accessToke, string refreshToken)
        {
            User = user;
            AccessToken = accessToke;
            RefreshToken = refreshToken;
        }

        public User User { get; }

        public string AccessToken { get; }

        public string RefreshToken { get; }
    }
}
