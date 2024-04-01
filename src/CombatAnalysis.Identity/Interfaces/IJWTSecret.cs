namespace CombatAnalysis.Identity.Interfaces;

public interface IJWTSecret
{
    public Task GenerateSecretKeysAsync();

    public Task<CustomerDAL.Entities.Authentication.Secret> GetSecretAsync();
}
