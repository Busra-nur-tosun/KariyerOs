using Application.Common.Security;
using Domain.Modules.Users.Entities;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    AccessTokenResult GenerateAccessToken(User user);
}
