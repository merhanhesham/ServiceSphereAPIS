using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.APIs.Errors;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.repositery.Data;
using System.Security.Claims;

namespace ServiceSphere.APIs.Controllers
{
    
    public class ClientController : BaseController
    {
        private readonly ServiceSphereContext _serviceSphereContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ClientController(ServiceSphereContext serviceSphereContext,UserManager<AppUser> userManager,IMapper mapper)
        {
            _serviceSphereContext = serviceSphereContext;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpPut("UpdateProfile/{ClientId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateProfile([FromBody] ClientProfileDto clientDto, string ClientId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }

            clientDto.UserId = user.Id;

            var Client = await _serviceSphereContext.Clients.FindAsync(ClientId);
            if (Client == null)
            {
                return NotFound(new ApiResponse(404, "freelancer doesn't exist"));
            }

            // Update the user's phone number using UserManager
            user.PhoneNumber = clientDto.PhoneNumber;
            var resultForUserManager = await _userManager.UpdateAsync(user);
            if (!resultForUserManager.Succeeded)
            {
                // Handle the error if the update fails
                return StatusCode(500, $"An error occurred while updating the user's phone number: {string.Join(", ", resultForUserManager.Errors.Select(e => e.Description))}");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the changes from proposalDto to the tracked entity directly
                _mapper.Map(clientDto, Client); // This avoids creating a new instance

                // _proposalRepo.Update(proposal); // This line might be unnecessary if _mapper.Map updates the tracked entity
                var result = await _serviceSphereContext.SaveChangesAsync();
                if (result <= 0) { return null; }
                return Ok(clientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }

        [HttpGet("GetProfile/{ClientId}")]
        public async Task<IActionResult> GetClientProfile(string ClientId)
        {

            var Client = await _serviceSphereContext.Clients
                .Include(F => F.Reviews)
                .FirstOrDefaultAsync(C=>C.Id==ClientId);
            var MappedClient = _mapper.Map<ClientProfileToReturnDto>(Client);


            if (Client == null)
            {

                return NotFound(new ApiResponse(404, "No Client found."));
            }

            // You might want to map these entities to DTOs before returning them
            return Ok(MappedClient);
        }

    }
}
