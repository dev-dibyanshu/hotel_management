using IdentityService.Application.DTOs;

namespace IdentityService.Application.Interfaces;

public interface IIdentityService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task<UserResponseDto?> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
}
