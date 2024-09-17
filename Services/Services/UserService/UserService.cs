using Core;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Services.OrderService.Dto;
using Services.Services.TokenService;
using Services.Services.UserService.Dto;
using System.Security.Claims;

namespace Services.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly AppIdentityDbContext _context;

        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
             AppIdentityDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }


        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user),
                Roles = roles.ToList()
            };
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);

            if (user != null)
                return null;

            var appUser = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
            };

            var result = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!result.Succeeded)
                return null;

            await _userManager.AddToRoleAsync(appUser, "User");

            var roles = await _userManager.GetRolesAsync(appUser);

            return new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Token = await _tokenService.CreateTokenAsync(appUser),
                Roles = roles.ToList()
            };
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var userId = _userManager.GetUserId(ClaimsPrincipal.Current);

            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user),
                Address = await GetUserAddress(userId),
                Roles = roles.ToList()
            };
        }

        public async Task<AddressDto> GetUserAddress(string userId)
        {

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AppUserId == userId);
            if (address == null)
                return null;

            return new AddressDto
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode
            };
        }

        public async Task<AddressDto> UpdateUserAddress(string userId, AddressDto addressDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return null;
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AppUserId == userId);
            if (address == null)
            {
                address = new Address
                {
                    AppUserId = userId,
                    FirstName = addressDto.FirstName ?? "Default FirstName",
                    LastName = addressDto.LastName ?? "Default LastName",
                    Street = addressDto.Street ?? "Default Street",
                    City = addressDto.City ?? "Default City",
                    State = addressDto.State ?? "Default State",
                    ZipCode = addressDto.ZipCode ?? "00000"
                };
                _context.Addresses.Add(address);
            }
            else
            {
                address.FirstName = addressDto.FirstName ?? address.FirstName;
                address.LastName = addressDto.LastName ?? address.LastName;
                address.Street = addressDto.Street ?? address.Street;
                address.City = addressDto.City ?? address.City;
                address.State = addressDto.State ?? address.State;
                address.ZipCode = addressDto.ZipCode ?? address.ZipCode;
            }

            await _context.SaveChangesAsync();

            return new AddressDto
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode
            };
        }

        public async Task<bool> UpdateUserRole(UpdateUserRoleDto updateUserRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(updateUserRoleDto.Email);

            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            var addResult = await _userManager.AddToRoleAsync(user, updateUserRoleDto.NewRole);
            if (!addResult.Succeeded)
                return false;

            return true;
        }

        public async Task<IReadOnlyList<UsersToShowDTO>> GetAllUsers()
        {
            var newUsersDto = new List<UsersToShowDTO>();

            var users = _context.Users.ToList();


            foreach (var user in users) 
            {
                var roles = await _userManager.GetRolesAsync(user);

                var newUser = new UsersToShowDTO
                {
                    userId = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Roles = roles.ToList()
                };
                newUsersDto.Add(newUser);
            }
            return newUsersDto;
        }

        public async Task<IReadOnlyList<IdentityRole>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            return roles;
        }

        public async Task<UsersToShowDTO> GetUserData(string userId)
        {
            var user = _context.Users.Find(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var userData = new UsersToShowDTO
            {
                userId = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Roles = roles.ToList(),
            };
            return userData;
        }
    }
}
