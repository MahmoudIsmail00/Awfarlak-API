using Awfarlak_API.HandleResponses;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Services.OrderService.Dto;
using Services.Services.UserService;
using Services.Services.UserService.Dto;
using System.Security.Claims;

namespace Awfarlak_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserService userService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.Register(registerDto);

            if (user == null)
                return BadRequest(new ApiException(400, "Email already Exist"));

            return Ok(user);

        }

        [HttpPost]

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.Login(loginDto);

            if (user == null)
                return Unauthorized(new ApiResponse(401));

            return Ok(user);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {

            return Ok("This is admin data");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto updateUserRoleDto)
        {
            var result = await _userService.UpdateUserRole(updateUserRoleDto);
            if (!result)
                return BadRequest(new ApiException(400, "Failed to update user role"));

            return Ok("User role updated successfully");
        }



        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiException(401, "User is not authenticated"));

            var address = await _userService.GetUserAddress(userId);

            if (address == null)
                return NotFound(new ApiException(404, "Address not found"));

            return Ok(address);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateAddress([FromBody] AddressDto addressDto)
        {


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.UpdateUserAddress(userId, addressDto);

            if (result == null)
                return BadRequest(new ApiException(400, "Address update failed"));

            return NoContent();
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiException(401, "User is not authenticated"));

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound(new ApiException(404, "User not found"));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
            };
        }
    }
}
