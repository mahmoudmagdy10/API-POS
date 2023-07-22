using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.API.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Errors;
using Talabat.Core.IService;

namespace Talabat.API.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("~/Api/Account/Login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO LoginDto)
        {
            var user = await _userManager.FindByEmailAsync(LoginDto.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, LoginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user,_userManager),
            });
        }

        [HttpPost]
        [Route("~/Api/Account/Register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO RegisterDto)
        {
            var CheckUser = await _userManager.FindByEmailAsync(RegisterDto.Email);
            if (CheckUser is not null) return BadRequest(new ApiResponse(400, "This Email Was Take"));

            if (CheckUserAddress(RegisterDto.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email Is In use !! " } });

            var user = new ApplicationUser()
            {
                DisplayName = RegisterDto.DisplayName,
                Email = RegisterDto.Email,
                PhoneNumber = RegisterDto.PhoneNumber,
                UserName = RegisterDto.Email.Split("@")[0],
            };
            var result = await _userManager.CreateAsync(user, RegisterDto.Password);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            });
        }

        [HttpPost]
        [Route("~/Api/Account/CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            });
        }

        [HttpGet]
        [Authorize]
        [Route("~/Api/Account/GetUserAddress")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var user = await _userManager.FindUserAddressByEmailAsync(User);
            var address = _mapper.Map<Address, AddressDTO>(user.Address);
            return Ok(address);
        }
        
        [HttpPut]
        [Authorize]
        [Route("~/Api/Account/UpdateUserAddress")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO NewAddress)
        {
            var user = await _userManager.FindUserAddressByEmailAsync(User);
            var address = _mapper.Map<AddressDTO, Address>(NewAddress);

            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(NewAddress);
        }
        

        [HttpGet("~/Api/Account/CheckUserAddress")]
        public async Task<ActionResult<bool>> CheckUserAddress(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
