﻿namespace CombatAnalysis.IdentityDAL.Entities;

public class IdentityUser
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public bool EmailVerified { get; set; }
}
