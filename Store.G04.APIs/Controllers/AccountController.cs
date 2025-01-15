using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Store.G04.APIs.Errors;
using Store.G04.core.Dtos.Auth;
using Store.G04.core.Entities.Identity;
using Store.G04.core.Services.Contract;
using Store.G04.Service.Services.Tokens;
using System.Security.Claims;

namespace Store.G04.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(IUserService userService
            ,UserManager<AppUser> userManager
            ,ITokenService tokenService)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("login")]//Post : /api/Accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user == null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);
        }
        [HttpPost("register")]//Post : /api/Accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid Signup"));
            return Ok(user);
        }
        [HttpGet("GetCurrentUser")]//Get : /api/Accounts/GetCurrentUser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
    }
}
