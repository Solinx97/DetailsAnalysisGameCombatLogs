using System;

namespace CombatAnalysis.UploadingLogsApp.Models.User;

public class AppUserModel
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string AboutMe { get; set; }

    public int Gender { get; set; }

    public string IdentityUserId { get; set; }
}
