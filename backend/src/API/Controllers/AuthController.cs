using API.Common.Options;
using API.Common.Models;
using Application.Common.Interfaces;
using Application.Modules.Auth.Commands.Login;
using Application.Modules.Auth.Commands.RefreshToken;
using Application.Modules.Auth.Commands.Register;
using Application.Modules.Auth.Models;
using Domain.Modules.Users.Entities;
using Domain.Modules.Users.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using UserEntity = Domain.Modules.Users.Entities.User;
using RefreshTokenEntity = Domain.Modules.Auth.Entities.RefreshToken;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    ISender sender,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IRefreshTokenGenerator refreshTokenGenerator,
    IDateTimeProvider dateTimeProvider,
    IAuthOptions authOptions,
    IOptions<FrontendOptions> frontendOptions,
    IOptions<GoogleAuthOptions> googleOptions) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.ConfirmPassword,
                request.Role,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"),
            cancellationToken);

        return Ok(new ApiResponse<AuthResponse>(response));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new LoginCommand(
                request.Email,
                request.Password,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"),
            cancellationToken);

        return Ok(new ApiResponse<AuthResponse>(response));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new RefreshTokenCommand(
                request.RefreshToken,
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"),
            cancellationToken);

        return Ok(new ApiResponse<AuthResponse>(response));
    }

    [HttpGet("google/start")]
    public IActionResult GoogleStart([FromQuery] string? role = null)
    {
        if (!googleOptions.Value.IsConfigured)
        {
            return Problem(
                title: "Google sign-in is not configured.",
                detail: "Add Google ClientId and ClientSecret values in API configuration to enable this flow.",
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        var requestedRole = NormalizeRole(role);
        var redirectUri = Url.ActionLink(nameof(GoogleCallback), values: new { role = requestedRole })!;
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUri
        };

        return Challenge(properties, "Google");
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string? role, CancellationToken cancellationToken)
    {
        var authenticationResult = await HttpContext.AuthenticateAsync("External");

        if (!authenticationResult.Succeeded || authenticationResult.Principal is null)
        {
            return Redirect(BuildFrontendCallbackUrl("Google sign-in could not be completed."));
        }

        var email = authenticationResult.Principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            await HttpContext.SignOutAsync("External");
            return Redirect(BuildFrontendCallbackUrl("Google account email could not be resolved."));
        }

        var firstName = authenticationResult.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "Google";
        var lastName = authenticationResult.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User";
        var normalizedEmail = email.Trim().ToUpperInvariant();
        var requestedRole = NormalizeRole(role);

        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null)
        {
            user = UserEntity.Create(
                firstName,
                lastName,
                email,
                passwordHasher.HashPassword(Guid.NewGuid().ToString("N")),
                requestedRole);

            await userRepository.AddAsync(user, cancellationToken);
        }

        var rawRefreshToken = refreshTokenGenerator.Generate();
        var refreshToken = RefreshTokenEntity.Create(
            user.Id,
            refreshTokenGenerator.Hash(rawRefreshToken),
            dateTimeProvider.UtcNow.AddDays(authOptions.RefreshTokenExpirationDays),
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");

        user.AddRefreshToken(refreshToken, dateTimeProvider.UtcNow);
        await userRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        user.RemoveExpiredRefreshTokens(dateTimeProvider.UtcNow);

        await userRepository.SaveChangesAsync(cancellationToken);
        await HttpContext.SignOutAsync("External");

        var accessToken = tokenService.GenerateAccessToken(user);

        return Redirect(BuildFrontendCallbackUrl(new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role,
            accessToken.Token,
            accessToken.ExpiresAtUtc,
            rawRefreshToken,
            refreshToken.ExpiresAtUtc)));
    }

    private string BuildFrontendCallbackUrl(AuthResponse response)
    {
        var fragment = string.Join("&", new[]
        {
            $"accessToken={Uri.EscapeDataString(response.AccessToken)}",
            $"accessTokenExpiresAtUtc={Uri.EscapeDataString(response.AccessTokenExpiresAtUtc.ToString("O"))}",
            $"refreshToken={Uri.EscapeDataString(response.RefreshToken)}",
            $"refreshTokenExpiresAtUtc={Uri.EscapeDataString(response.RefreshTokenExpiresAtUtc.ToString("O"))}"
        });

        return $"{frontendOptions.Value.BaseUrl}/auth/google-callback#{fragment}";
    }

    private string BuildFrontendCallbackUrl(string error) =>
        $"{frontendOptions.Value.BaseUrl}/auth/google-callback#error={Uri.EscapeDataString(error)}";

    private static string NormalizeRole(string? role) =>
        role is SystemRoles.Employer ? SystemRoles.Employer : SystemRoles.Candidate;
}

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword,
    string Role);

public sealed record LoginRequest(string Email, string Password);

public sealed record RefreshRequest(string RefreshToken);
