using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.APIs.Errors;
using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Posting;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.core.Specifications;
using ServiceSphere.repositery.Data;
using System.Collections;
using System.Security.Claims;
using static ServiceSphere.core.Specifications.ProposalSpecs;

namespace ServiceSphere.APIs.Controllers
{

    public class ServicesController : BaseController
    {
        private readonly IGenericRepositery<Category> _categoryRepositery;
        private readonly IGenericRepositery<SubCategory> _subCategoryRepositery;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepositery<Service> _serviceRepo;
        private readonly IMapper _mapper;
        private readonly ServiceSphereContext _serviceSphereContext;

        public ServicesController(IGenericRepositery<Category> categoryRepositery, IGenericRepositery<SubCategory> SubCategoryRepositery, UserManager<AppUser> userManager, IGenericRepositery<Service> serviceRepo, IMapper mapper, ServiceSphereContext serviceSphereContext)
        {
            _categoryRepositery = categoryRepositery;
            _subCategoryRepositery = SubCategoryRepositery;
            _userManager = userManager;
            _serviceRepo = serviceRepo;
            _mapper = mapper;
            _serviceSphereContext = serviceSphereContext;
        }
        [HttpGet("Category")]
        public async Task<ActionResult<Category>> GetCategoriesAsync()
        {
            var Categories = await _categoryRepositery.GetAllAsync();
            return Ok(Categories);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Category>> GetByIdAsync(int Id)
        {
            var Category = await _categoryRepositery.GetByIdAsync(Id);
            if (Category is null) return NotFound();
            return Ok(Category);
        }
        [HttpGet("SubCategory")]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesAsync()
        {
            var Spec = new CategoryAndSubCategorySpec();
            var SubCategories = await _subCategoryRepositery.GetAllWithSpecAsync(Spec);
            return Ok(SubCategories);
        }


        [HttpPost("AddService")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddService([FromBody] ServiceDto serviceDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            //if (serviceDto.UserId != user.Id)
            //{
            //    return Unauthorized(new ApiResponse(404, "You Mustn't submit a service"));
            //}
            var Freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            serviceDto.FreelancerId = Freelancer.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                var mappedService = _mapper.Map<Service>(serviceDto);
                await _serviceRepo.AddAsync(mappedService);
                var result = await _serviceSphereContext.SaveChangesAsync();
                if (result <= 0) { return null; }
                return Ok(serviceDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }

        [HttpGet("GetUserServices")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserServicess()
        {

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(400, "user not recognized"));
            }

            var Freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            //serviceDto.FreelancerId = Freelancer.Id;
            //user.Id =;
            var spec = new ServiceSpec(Freelancer.Id);
            var Services = await _serviceRepo.GetAllWithSpecAsync(spec);
            var mappedServicess = _mapper.Map<List<ServiceToReturnDto>>(Services);


            if (Services == null || !Services.Any())
            {

                return NotFound(new ApiResponse(404, "No Services found for this user."));
            }

            // You might want to map these entities to DTOs before returning them
            return Ok(mappedServicess);
        }

        [HttpPut("UpdateService/{ServiceId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateService([FromBody] ServiceDto serviceDto, int ServiceId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            //handle userId
            //serviceDto.UserId = user.Id;
            var Freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            serviceDto.FreelancerId = Freelancer.Id;

            //var spec = new ProposalSpecs(PostId, postingType);
            //var proposal = await _proposalRepo.GetByIdWithSpecAsync(spec);
            //if (proposal == null)
            //{
            //    return NotFound($"Proposal doesn't exist");
            //}
            var Service = await _serviceRepo.GetByIdAsync(ServiceId);
            if (Service == null)
            {
                return NotFound($"Service doesn't exist");
            }
            // UpdateproposalDto.Id = proposal.Id;
            if (Service.FreelancerId != Freelancer.Id)
            {
                return Unauthorized(new ApiResponse(400, "You do not have permission to update this Service."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the changes from proposalDto to the tracked entity directly
                _mapper.Map(serviceDto, Service); // This avoids creating a new instance

                // _proposalRepo.Update(proposal); // This line might be unnecessary if _mapper.Map updates the tracked entity
                var result = await _serviceSphereContext.SaveChangesAsync();
                if (result <= 0) { return null; }
                return Ok(serviceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }

        [HttpDelete("DeleteService/{ServiceId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteService(int ServiceId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "user not found."));
            }

            var Service = await _serviceRepo.GetByIdAsync(ServiceId);
            if (Service == null)
            {
                return NotFound("Service not found.");
            }

            // Check if the user is authorized to delete the proposal
            // This is just an example; adjust the condition according to your authorization logic
            var Freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            if (Service.FreelancerId != Freelancer.Id)
            {
                return Unauthorized(new ApiResponse(400, "You do not have permission to delete this Service."));
            }

            try
            {
                // Perform the deletion
                _serviceRepo.Delete(Service);
                await _serviceSphereContext.SaveChangesAsync();
                return Ok("You deleted the Service successfully");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting: {ex.Message}");
            }
        }

        [HttpGet("GetService/{serviceId}")] // Adjust the route template as needed
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            // Assuming you have a repository or DbContext to access services
            var service = await _serviceSphereContext.Services
                .Include(s => s.Category) // Include related data as needed
                .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return NotFound(new ApiResponse(404, $"Service with ID {serviceId} not found."));
            }

            var serviceToReturn = _mapper.Map<ServiceToReturnDto>(service);
            return Ok(serviceToReturn);
        }


    }
}
