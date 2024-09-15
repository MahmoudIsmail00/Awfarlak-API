using Core;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Services.OrderService.Dto;
using Services.Services.TokenService;
using Services.Services.UserService.Dto;

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

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
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

            return new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser)
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



    }
}
