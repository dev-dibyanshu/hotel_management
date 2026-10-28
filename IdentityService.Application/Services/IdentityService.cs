using IdentityService.Application.DTOs;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Services;

public class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        UserManager<ApplicationUser> userManager)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetByUsernameAsync(loginRequest.Username);
        
        if (user == null)
            return null;

        var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        
        if (!isValidPassword)
            return null;

        var role = await _userRepository.GetUserRoleAsync(user);
        
        if (string.IsNullOrEmpty(role))
            return null;

        var token = _jwtTokenService.GenerateToken(user, role);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.UserName ?? string.Empty,
            Role = role
        };
    }

    public async Task<UserResponseDto?> CreateUserAsync(CreateUserDto createUserDto)
    {
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(createUserDto.Username);
        if (existingUserByUsername != null)
            return null;

        var existingUserByEmail = await _userRepository.GetByEmailAsync(createUserDto.Email);
        if (existingUserByEmail != null)
            return null;

        var newUser = new ApplicationUser
        {
            UserName = createUserDto.Username,
            Email = createUserDto.Email
        };

        var success = await _userRepository.CreateUserAsync(newUser, createUserDto.Password, createUserDto.Role);
        
        if (!success)
            return null;

        return new UserResponseDto
        {
            Id = newUser.Id,
            Username = newUser.UserName ?? string.Empty,
            Email = newUser.Email ?? string.Empty,
            Role = createUserDto.Role
        };
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
            return null;

        if (!string.IsNullOrEmpty(updateUserDto.Email))
        {
            var existingUserWithEmail = await _userRepository.GetByEmailAsync(updateUserDto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != id)
                return null;
            
            user.Email = updateUserDto.Email;
        }

        var currentRole = await _userRepository.GetUserRoleAsync(user);
        
        if (!string.IsNullOrEmpty(updateUserDto.Role) && updateUserDto.Role != currentRole)
        {
            if (!string.IsNullOrEmpty(currentRole))
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
            }
            await _userManager.AddToRoleAsync(user, updateUserDto.Role);
        }

        var success = await _userRepository.UpdateUserAsync(user);
        
        if (!success)
            return null;

        var updatedRole = await _userRepository.GetUserRoleAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = updatedRole ?? string.Empty
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userRepository.DeleteUserAsync(id);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
            return null;

        var role = await _userRepository.GetUserRoleAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Role = role ?? string.Empty
        };
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var role = await _userRepository.GetUserRoleAsync(user);
            userDtos.Add(new UserResponseDto
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = role ?? string.Empty
            });
        }

        return userDtos;
    }
}
