namespace CombatAnalysisIdentity.Models;

public class AuthorizationRequestModel
{
     public string RedirectUri { get; set; }

     public string GrantType { get; set; }

     public string ClientTd { get; set; }

     public string Scope { get; set; }

     public string State { get; set; }

     public string CodeChallengeMethod { get; set; }

     public string CodeChallenge { get; set; }
}
