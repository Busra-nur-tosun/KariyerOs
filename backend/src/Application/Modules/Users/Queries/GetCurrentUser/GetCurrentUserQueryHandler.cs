using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Modules.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    public async Task<CurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        return new CurrentUserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role);
    }
}
