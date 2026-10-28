using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, string role);
}
