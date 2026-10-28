using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IIdentityService identityService, ILogger<UsersController> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _identityService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { message = "An error occurred while retrieving users" });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _identityService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var validRoles = new[] { "Owner", "Manager", "Receptionist" };
            if (!validRoles.Contains(createUserDto.Role))
                return BadRequest(new { message = "Invalid role. Must be Owner, Manager, or Receptionist" });

            var user = await _identityService.CreateUserAsync(createUserDto);

            if (user == null)
                return Conflict(new { message = "Username or email already exists" });

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, new { message = "An error occurred while creating the user" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Owner,Manager")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!string.IsNullOrEmpty(updateUserDto.Role))
            {
                var validRoles = new[] { "Owner", "Manager", "Receptionist" };
                if (!validRoles.Contains(updateUserDto.Role))
                    return BadRequest(new { message = "Invalid role. Must be Owner, Manager, or Receptionist" });
            }

            var user = await _identityService.UpdateUserAsync(id, updateUserDto);

            if (user == null)
                return NotFound(new { message = "User not found or email already exists" });

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the user" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var success = await _identityService.DeleteUserAsync(id);

            if (!success)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the user" });
        }
    }
}
