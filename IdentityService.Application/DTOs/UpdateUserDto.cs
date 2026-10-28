using System.ComponentModel.DataAnnotations;

namespace IdentityService.Application.DTOs;

public class UpdateUserDto
{
    [EmailAddress]
    public string? Email { get; set; }

    public string? Role { get; set; }
}
