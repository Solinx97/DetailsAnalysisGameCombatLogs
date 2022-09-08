using System;

namespace CombatAnalysis.WebApp.Models.Authentication
{
    public class RefreshTokenModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}
