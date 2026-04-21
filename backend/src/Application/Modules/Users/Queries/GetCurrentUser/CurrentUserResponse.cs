namespace Application.Modules.Users.Queries.GetCurrentUser;

public sealed record CurrentUserResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role);
