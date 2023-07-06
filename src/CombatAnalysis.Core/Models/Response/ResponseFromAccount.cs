using CombatAnalysis.Core.Models.User;

namespace CombatAnalysis.Core.Models.Response;

public struct ResponseFromAccount
{
    public AppUserModel User { get; set;  }

    public string AccessToken { get; set;  }

    public string RefreshToken { get; set; }
}
