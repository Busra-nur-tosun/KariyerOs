using Application.Common.Interfaces;
using Domain.Modules.Users.Entities;
using Domain.Modules.Users.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication;

public sealed class PasswordHasherAdapter : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(CreateProxyUser(), password);
    }

    public bool VerifyPassword(string passwordHash, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(CreateProxyUser(), passwordHash, providedPassword);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }

    private static User CreateProxyUser()
    {
        return User.Create("System", "User", "placeholder@kariyeros.local", "placeholder", SystemRoles.Candidate);
    }
}
