namespace CombatAnalysis.UserBL.DTO;

public record AppUserDto(
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