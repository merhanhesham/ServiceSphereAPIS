using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.APIs.Errors;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Entities.Users.Freelancer;
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.core.Specifications;
using ServiceSphere.core.SpecificationsForUsers;
using ServiceSphere.repositery.Data;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace ServiceSphere.APIs.Controllers
{ 
    public class FreelancerController : BaseController
    {
        private readonly ServiceSphereContext _serviceSphereContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public FreelancerController(ServiceSphereContext serviceSphereContext,IMapper mapper,UserManager<AppUser> userManager)
        {
            _serviceSphereContext = serviceSphereContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("GetProfile")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFreelancerProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)

            {

                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            var freelancer = await _serviceSphereContext.Freelancers.Where(f=>f.Email == email).FirstOrDefaultAsync();
            if(freelancer == null) { return NotFound(new ApiResponse(404, "freelancer not found.")); }

                var Freelancer = await _serviceSphereContext.Freelancers.Include(F=>F.Categories)
                .Include(F=>F.SubCategories)
                .Include(F=>F.Services)
                .Include(F=>F.Reviews)
                .Include(F=>F.Skills)
                .FirstOrDefaultAsync(F=>F.Id==freelancer.Id);
            var MappedFreelancer = _mapper.Map<FreelancerProfileToReturnDto>(Freelancer);


            if (Freelancer == null )
            {

                return NotFound(new ApiResponse(404, "No Freelancer found."));
            }

            // You might want to map these entities to DTOs before returning them
            return Ok(MappedFreelancer);
        }

       
        [HttpPut("UpdateProfile")]

        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> UpdateProfile([FromBody] FreelancerProfileDto freelancerProfileDto, string? Email)

        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            Email = email;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)

            {

                return NotFound(new ApiResponse(404, "Target user not found."));

            }
            

           // var freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            //freelancerProfileDto.UserId = freelancer.Id;

            //handle diff ids in aspnetusers, app user
            //var Freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            //freelancerProfileDto.UserId = user.Id;
            var freelancer = await _serviceSphereContext.Freelancers

                .Include(f => f.Categories) // Ensure Categories are included to update them

                .Include(f => f.SubCategories) // Ensure SubCategories are also included for update

                .FirstOrDefaultAsync(f => f.Email == email);


            //freelancer.Id = user.Id;

            if (freelancer == null)

            {

                return NotFound(new ApiResponse(404, "Freelancer doesn't exist"));

            }

            user.PhoneNumber = freelancerProfileDto.PhoneNumber;
            //freelancerProfileDto.UserId = FreelancerId;

            var resultForUserManager = await _userManager.UpdateAsync(user);

            if (!resultForUserManager.Succeeded)

            {

                return StatusCode(500, $"An error occurred while updating the user's phone number: {string.Join(", ", resultForUserManager.Errors.Select(e => e.Description))}");

            }

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }

            // Clear existing categories and subcategories

            freelancer.Categories.Clear();

            freelancer.SubCategories.Clear();

            // Fetch and add new categories based on CategoryIds

            var categoriesToAdd = await _serviceSphereContext.Categories

                .Where(c => freelancerProfileDto.CategoryIds.Contains(c.Id))

                .ToListAsync();

            foreach (var category in categoriesToAdd)

            {

                freelancer.Categories.Add(category);

            }

            // Fetch and add new subcategories based on SubCategoryIds

            if (freelancerProfileDto.SubCategoryIds != null && freelancerProfileDto.SubCategoryIds.Any())

            {

                var subCategoriesToAdd = await _serviceSphereContext.SubCategories

                    .Where(sc => freelancerProfileDto.SubCategoryIds.Contains(sc.Id))

                    .ToListAsync();

                foreach (var subCategory in subCategoriesToAdd)

                {

                    freelancer.SubCategories.Add(subCategory);

                }

            }

            _mapper.Map(freelancerProfileDto, freelancer); // Map other changes

            try

            {

                var result = await _serviceSphereContext.SaveChangesAsync();

                if (result <= 0)

                {

                    return StatusCode(500, "An error occurred while saving the profile.");

                }

                return Ok(freelancerProfileDto);

            }

            catch (Exception ex)

            {

                return StatusCode(500, $"An error occurred while saving the profile: {ex.Message}");

            }

        }


        [HttpGet("Freelancers")]
        public async Task<IActionResult> GetFreelancers([FromQuery] FreelancerSpecParams? freelancerParams)
        {
            // Initialize query
            IQueryable<Freelancer> query = _serviceSphereContext.Freelancers.Include(f => f.Categories).Include(f => f.SubCategories).Include(f=>f.Services);

            // Apply filters if provided
            if (freelancerParams != null)
            {
                if (freelancerParams.CategoryId.HasValue)
                {
                    query = query.Where(f => f.Categories.Any(c => c.Id == freelancerParams.CategoryId));
                }
                if (freelancerParams.SubCategoryId.HasValue)
                {
                    query = query.Where(f => f.SubCategories.Any(sc => sc.Id == freelancerParams.SubCategoryId));
                }
                if (!string.IsNullOrEmpty(freelancerParams.search))
                {
                    query = query.Where(f => f.DisplayName.ToLower().Contains(freelancerParams.search));
                }
            }

            // Execute the query to get the freelancers
            var freelancers = await query.ToListAsync();

            // Check if freelancers were found
            if (!freelancers.Any())
            {
                return NotFound(new ApiResponse(404, "No freelancers found."));
            }

            // Map the entities to DTOs before returning
            var mappedFreelancers = _mapper.Map<List<FreelancerDto>>(freelancers);
            return Ok(mappedFreelancers);
        }

    }
}
