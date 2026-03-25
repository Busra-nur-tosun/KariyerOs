using Application.Common.Models;
using MediatR;

namespace Application.Common.Queries;

public sealed record GetApplicationInfoQuery(string Environment) : IRequest<ApplicationInfoDto>;
