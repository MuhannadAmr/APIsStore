using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.DEMO.Core.Dtos.Auth;
using Store.DEMO.Core.Entites.Identity;
using Store.DEMO.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Service.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }


        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) return null;
            var pass = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!pass.Succeeded) return null;

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = loginDto.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExitsAsync(registerDto.Email)) return null;

            var user = new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return null;

            return new UserDto()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }

        public async Task<bool> CheckEmailExitsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto newAddress)
        {
            if (email is null || newAddress is null) return null;
            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
            if (user is null) return null;
            user.Address = _mapper.Map<Address>(newAddress);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return null;
            return _mapper.Map<AddressDto>(user.Address);
        }
    }
}
