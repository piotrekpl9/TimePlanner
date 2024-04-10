using Domain.User.ValueObjects;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserId userId, string name, string surname);
}