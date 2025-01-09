using Application.Services.OrderService.Dto;
using Application.Services.UserService.Dto;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> Login(LoginDto loginDto);

        Task<IReadOnlyList<UsersToShowDTO>> GetAllUsers();
        Task<IReadOnlyList<IdentityRole>> GetAllRoles();
        Task<UsersToShowDTO> GetUserData(string userId);

        Task<AddressDto> UpdateUserAddress(string userId, AddressDto addressDto);
        Task<AddressDto> GetUserAddress(string userId);
        Task<bool> UpdateUserRole(UpdateUserRoleDto updateUserRoleDto);
        Task<UserDto> GetCurrentUser();
        Task<UserDto> ChangeDetails(string userId, UserChangePasswordDto userToChange);
    }
}
