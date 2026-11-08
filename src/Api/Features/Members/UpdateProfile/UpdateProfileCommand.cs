using Domain.Common;
using Mediator;

namespace Api.Features.Members.UpdateProfile;

public sealed record UpdateProfileCommand(
    string City,
    string Country,
    string? Introduction,
    string? LookingFor,
    string? Interests) : ICommand<Result>
{
    public Guid UserId { get; init; }
}