using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIS.Extensions;
using TalabatApi.Errors;
using TalabatCore.DTOs;
using TalabatCore.Entities;
using TalabatCore.Services;


namespace TalabatApi.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        #region Register

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register (RegisterDto model)
        {

            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "The Email is already in use"));

            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0]
            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var returnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };

            return Ok(returnedUser);
        }

        #endregion

        #region Login

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        #endregion

        #region Get Current User

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email); 
            var user = await _userManager.FindByEmailAsync(email); 

            if (user is null) return Unauthorized(new ApiResponse(401)); 

            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
        }


        #endregion

        #region Get Current User Address

        [Authorize]
        [HttpGet("Address")]
        public async Task <ActionResult<Address>> GetCurrentAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var mappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(user.Address);
        }

        #endregion

        #region Update User Address

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto updatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedAddress);
        }

        #endregion

        #region Validate Email

        [HttpGet("emailExists")]
        public async Task <ActionResult<bool>> CheckEmailExists(string email)
            
            => await _userManager.FindByEmailAsync(email) is not null;

        #endregion
    }
}
