using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.APIs.Errors;
using Store.DEMO.APIs.Extenstions;
using Store.DEMO.Core.Dtos.Auth;
using Store.DEMO.Core.Entites.Identity;
using Store.DEMO.Core.Services.Contract;
using System.Security.Claims;

namespace Store.DEMO.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.LoginAsync(loginDto);
            if (user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized, "InValid Login !!"));
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "InValid Register !!"));
            return Ok(user);
        }
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                Email = userEmail,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }
        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress()
        {

            var user = await _userManager.FindByEmailWithAddressAsync(User);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [HttpPost("ChangeAddress")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto newAddress)
        {
            if (newAddress is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var result = await _userService.UpdateCurrentUserAddressAsync(userEmail, newAddress);
            if (result is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return result;
        }

    }
}
