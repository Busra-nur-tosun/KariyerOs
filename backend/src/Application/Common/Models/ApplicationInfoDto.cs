namespace Application.Common.Models;

public sealed record ApplicationInfoDto(
    string Name,
    string Environment,
    DateTime StartedAtUtc,
    IReadOnlyCollection<string> Modules);
