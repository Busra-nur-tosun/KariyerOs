namespace Application.Common.Interfaces;

public interface IAuthOptions
{
    int RefreshTokenExpirationDays { get; }
}
