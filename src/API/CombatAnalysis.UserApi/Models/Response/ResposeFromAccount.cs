using CombatAnalysis.UserApi.Models.User;

namespace CombatAnalysis.UserApi.Models.Response
{
    public struct ResponseFromAccount
    {
        public ResponseFromAccount(UserModel user, string accessToke, string refreshToken)
        {
            User = user;
            AccessToken = accessToke;
            RefreshToken = refreshToken;
        }

        public UserModel User { get; }

        public string AccessToken { get; }

        public string RefreshToken { get; }
    }
}
