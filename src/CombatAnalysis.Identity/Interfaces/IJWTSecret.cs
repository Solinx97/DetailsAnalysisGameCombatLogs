namespace CombatAnalysis.Identity.Interfaces;

public interface IJWTSecret
{
    public Task GenerateSecretKeysAsync();

    public Task<DAL.Entities.Authentication.Secret> GetSecretAsync();
}
