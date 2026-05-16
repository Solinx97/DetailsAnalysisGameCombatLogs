namespace CombatAnalysis.UserDAL.Entities;

public record AppUser(
    string Id,
    string Username,
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateTimeOffset Birthday,
    string AboutMe,
    int Gender,
    string IdentityUserId
    );
