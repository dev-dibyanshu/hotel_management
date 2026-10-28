using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    // Role will be managed through Identity's role system
    // Base IdentityUser<int> already provides:
    // - Id (int)
    // - UserName
    // - Email
    // - PasswordHash
}
