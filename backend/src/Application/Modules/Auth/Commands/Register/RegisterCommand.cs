using Application.Modules.Auth.Models;
using MediatR;

namespace Application.Modules.Auth.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string Role,
    string IpAddress) : IRequest<AuthResponse>;
