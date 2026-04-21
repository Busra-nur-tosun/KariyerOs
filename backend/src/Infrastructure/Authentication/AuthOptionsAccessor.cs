using Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

public sealed class AuthOptionsAccessor(IOptions<JwtOptions> options) : IAuthOptions
{
    public int RefreshTokenExpirationDays => options.Value.RefreshTokenExpirationDays;
}
