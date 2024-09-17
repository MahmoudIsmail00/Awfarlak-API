using Microsoft.AspNetCore.Identity;
using Services.Services.OrderService.Dto;
using Services.Services.UserService.Dto;

namespace Services.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> Login(LoginDto loginDto);

        Task<IReadOnlyList<UsersToShowDTO>> GetAllUsers();
        Task<IReadOnlyList<IdentityRole>> GetAllRoles();
        Task<UsersToShowDTO>  GetUserData(string userId);

        Task<AddressDto> UpdateUserAddress(string userId, AddressDto addressDto);
        Task<AddressDto> GetUserAddress(string userId);
        Task<bool> UpdateUserRole(UpdateUserRoleDto updateUserRoleDto);
        Task<UserDto> GetCurrentUser();

    }
}
