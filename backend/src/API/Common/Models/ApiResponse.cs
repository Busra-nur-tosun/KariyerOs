namespace API.Common.Models;

public sealed record ApiResponse<T>(T Data, string? Message = null);
