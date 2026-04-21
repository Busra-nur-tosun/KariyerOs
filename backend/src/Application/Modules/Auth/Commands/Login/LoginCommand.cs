using Application.Modules.Auth.Models;
using MediatR;

namespace Application.Modules.Auth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password,
    string IpAddress) : IRequest<AuthResponse>;
