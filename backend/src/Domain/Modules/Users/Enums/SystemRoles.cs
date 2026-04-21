namespace Domain.Modules.Users.Enums;

public static class SystemRoles
{
    public const string Candidate = "Candidate";
    public const string Employer = "Employer";
    public const string Admin = "Admin";

    public static readonly IReadOnlyCollection<string> All = [Candidate, Employer, Admin];
}
