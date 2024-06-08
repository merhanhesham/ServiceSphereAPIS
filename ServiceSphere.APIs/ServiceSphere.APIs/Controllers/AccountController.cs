using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.APIs.Errors;
using ServiceSphere.APIs.Extensions;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Entities.Users.Freelancer;
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.core.Services.contract;
using ServiceSphere.repositery.Data;
using System.Security.Claims;

namespace ServiceSphere.APIs.Controllers
{
    
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ServiceSphereContext _serviceSphereContext;
        
        public AccountController(UserManager<AppUser>userManager,SignInManager<AppUser>signInManager, IAuthService authService,IMapper mapper,ServiceSphereContext serviceSphereContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
            _serviceSphereContext = serviceSphereContext;
           
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) { return Unauthorized(new ApiResponse(401)); }
            //takes user and password, checks pass to sign in if true
            var result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);//false>> i don't want to lock acc if pass is false
            if (!result.Succeeded) { return Unauthorized(new ApiResponse(401)); }
            return Ok(new UserDto()
            {
                Id=User.Id,
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _authService.CreateTokenAsync(User, _userManager)
            }) ;;


        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model) //recieves obj of registerDto, return obj of userDto
        {
            if (CheckEmailExists(model.Email).Result.Value) { return BadRequest(new ApiResponse(400, "this email already exists")); }

            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(User, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400));
            }

            //roles
            var roleResult = await _userManager.AddToRoleAsync(User, model.Role); // Ensure `model` includes a Role property
            if (!roleResult.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to assign user role"));
            }

            

            if (model.Role.ToLower() == "client")
            {
                var client = new Client()
                {
                    DisplayName = User.DisplayName,
                    UserType = UserType.Client,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                };
                await _serviceSphereContext.Clients.AddAsync(client);
                await _serviceSphereContext.SaveChangesAsync();
            }
            else if (model.Role.ToLower() == "freelancer")
            {
                var freelancer = new Freelancer()
                {
                    UserName=User.UserName,
                    DisplayName = User.DisplayName,
                    UserType = UserType.Freelancer,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber
                };
                await _serviceSphereContext.Freelancers.AddAsync(freelancer);
                await _serviceSphereContext.SaveChangesAsync();
            }

            var ReturnedUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _authService.CreateTokenAsync(User, _userManager)
            };
           
            return Ok(ReturnedUser);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        [HttpGet]
        [Route("freelancers")]
        public async Task<IActionResult> GetFreelancers()
        {
            var usersInFreelancerRole = await _userManager.GetUsersInRoleAsync("Freelancer");
            // Optionally, transform users to a DTO to avoid sending sensitive data
            return Ok(usersInFreelancerRole);
        }

        [HttpGet]
        [Route("clients")]
        public async Task<IActionResult> GetClients()
        {
            var usersInClientRole = await _userManager.GetUsersInRoleAsync("Client");
            // Optionally, transform users to a DTO to avoid sending sensitive data
            return Ok(usersInClientRole);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            return Ok(new UserDto()
            {
                Id=user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        /*[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Address")]
        public async Task<ActionResult<AddressDto>> CreateAddress(AddressDto model)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "User not found."));
            }

            // Assuming model does not contain an Id value for POST operations
            var address = _mapper.Map<Address>(model); // Map DTO to Address entity

            // Add the address to the user - Implement this method in your user manager or a related service
            var result = await _userManager.AddAddressToUserAsync(user, address);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to add the address"));
            }

            // Optionally map back to DTO if you need to return the created object (now with an Id)
            var createdAddressDto = _mapper.Map<AddressDto>(address);

            return CreatedAtAction(nameof(CreateAddress), new { id = address.Id }, createdAddressDto);
        }*/


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto model)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            //model.Id = user.Address.Id;
            var mappedAddress = _mapper.Map<Address>(model);

            user.Address = mappedAddress;
            var Result = await _userManager.UpdateAsync(user);
           // var saveResult = await _userManager.

            if (!Result.Succeeded) return BadRequest(new ApiResponse(400, "Failed to update the address"));
            return Ok(model);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto>(user.Address);
            if (MappedAddress is null) return NotFound(new ApiResponse(404, "Address is not found"));
            return MappedAddress;
        }
    }
}
