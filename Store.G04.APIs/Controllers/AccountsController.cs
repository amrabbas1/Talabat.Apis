﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Store.G04.APIs.Errors;
using Store.G04.APIs.Extenstions;
using Store.G04.core.Dtos.Auth;
using Store.G04.core.Entities.Identity;
using Store.G04.core.Services.Contract;
using Store.G04.Service.Services.Tokens;
using System.IO;
using System.Security.Claims;

namespace Store.G04.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(IUserService userService
            ,UserManager<AppUser> userManager
            ,ITokenService tokenService
            ,IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
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
            if(CheckEmailExists(registerDto.Email).Result.Value)
            {
                return BadRequest(new ApiErrorResponse(400, "Email Is Already Exist"));
            }

            var user = await _userService.RegisterAsync(registerDto);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid Signup"));
            return Ok(user);
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("Address")]//Get : /api/Accounts/Address
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);
            if (user == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [Authorize]
        [HttpPut("Address")]//Get : /api/Accounts/Address
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindByEmailWithAddressAsync(User);

            if(user.Address == null)
            {
                user.Address = new Address
                {
                    FName = addressDto.FName,
                    LName = addressDto.LName,
                    City = addressDto.City,
                    Street = addressDto.Street,
                    Country = addressDto.Country
                };
            }
            else
            {
                var mappedAddress = _mapper.Map<Address>(addressDto);
                mappedAddress.Id = user.Address.Id;
                user.Address = mappedAddress;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest
                ,"Failed to update address"));

            return Ok(addressDto);
        }

        [HttpGet("emailExists")]//baseurl/api/accounts/emailExists
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return false;
            else return true;
        }

    }
}
