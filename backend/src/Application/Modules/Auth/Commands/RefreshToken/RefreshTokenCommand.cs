using Application.Modules.Auth.Models;
using MediatR;

namespace Application.Modules.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken, string IpAddress) : IRequest<AuthResponse>;
