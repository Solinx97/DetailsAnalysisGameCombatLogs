using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserDAL.IntegrationTests.Factory;

internal static class AppUserTestDataFactory
{
    public static AppUser Create(
        string? id = null,
        string? username = null,
        string? firstName = null,
        string? lastName = null,
        string? phoneNumber = null,
        DateTimeOffset? birthday = null,
        string? aboutMe = null,
        int? gender = null,
        string? identityUserId = null)
    {
        return new AppUser(
            Id: id ?? Guid.NewGuid().ToString(),
            Username: username ?? $"user_{Guid.NewGuid():N}",
            FirstName: firstName ?? "John",
            LastName: lastName ?? "Doe",
            PhoneNumber: phoneNumber ?? "123456",
            Birthday: birthday ?? DateTimeOffset.UtcNow,
            AboutMe: aboutMe ?? "Test user",
            Gender: gender ?? 1,
            IdentityUserId: identityUserId ?? $"uid_{Guid.NewGuid():N}"
        );
    }
}
