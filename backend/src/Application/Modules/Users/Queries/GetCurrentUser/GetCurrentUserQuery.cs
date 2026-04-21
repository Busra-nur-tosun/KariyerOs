using MediatR;

namespace Application.Modules.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(Guid UserId) : IRequest<CurrentUserResponse>;
