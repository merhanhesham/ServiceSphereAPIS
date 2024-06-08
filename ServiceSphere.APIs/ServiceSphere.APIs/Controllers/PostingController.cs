using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.APIs.Errors;
using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Posting;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.core.Specifications;
using ServiceSphere.repositery.Data;
using System.Net.Mail;
using System.Security.Claims;

namespace ServiceSphere.APIs.Controllers
{
    public class PostingController : BaseController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepositery<ServicePosting> _servicePostingRepo;
        private readonly IGenericRepositery<ProjectPosting> _projectPostingRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepositery<Proposal> _proposalRepo;
        private readonly ServiceSphereContext _ServiceSphereContext;
        public PostingController(ServiceSphereContext serviceSphereContext, UserManager<AppUser> userManager, IGenericRepositery<ServicePosting> ServicePostingRepo, IGenericRepositery<ProjectPosting> ProjectPostingRepo, IMapper mapper, IGenericRepositery<Proposal> ProposalRepo)
        {

            _userManager = userManager;
            _servicePostingRepo = ServicePostingRepo;
            _projectPostingRepo = ProjectPostingRepo;
            _mapper = mapper;
            _proposalRepo = ProposalRepo;
            _ServiceSphereContext = serviceSphereContext;

        }

        [HttpPost("ServicePosting")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostServicePosting([FromBody] ServicePostingDto model)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Get current user
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            // Check if the user exists
            //var userExists = await _userManager.FindByIdAsync(model.UserId) != null;
            //if (!userExists)
            //{
            //    return NotFound($"User with ID {model.UserId} not found.");
            //}

            model.UserId = user.Id;



            //get clientid
            var Client = await _ServiceSphereContext.Clients.Where(C => C.Email == user.Email).FirstOrDefaultAsync();
            if (Client == null) return BadRequest(new ApiResponse(400, "Client can not be found"));
            model.ClientId = Client.Id;





            // Create new service posting
            var servicePosting = new ServicePosting
            {
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
                userID = user.Id,// Assuming UserId is a foreign key to the ApplicationUser table
                EmailAddress = Email,
                ClientId = Client.Id,
                Budget = model.Budget,
                Duration = model.Duration,
                Deadline = model.Deadline,
            };
            _ServiceSphereContext.ServicePostings.Add(servicePosting);

            // Save changes
            try
            {
                await _ServiceSphereContext.SaveChangesAsync();
                return Ok("Service posted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPut("ServicePosting/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateServicePosting(int id, [FromBody] ServicePostingDto model)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the service posting exists
            var servicePosting = await _ServiceSphereContext.ServicePostings.FindAsync(id);
            if (servicePosting == null)
            {
                return NotFound($"Service posting with ID {id} not found.");
            }
            model.Id = id;
            // Get current user
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _userManager.FindByEmailAsync(Email);
            if (currentUser == null)
            {
                return NotFound(new ApiResponse(404, "Current user not found."));
            }

            // Check if the current user is authorized to update the service posting
            if (servicePosting.userID != currentUser.Id)
            {
                return Forbid("You are not authorized to update this service posting.");
            }

            // Update service posting details
            _mapper.Map(model, servicePosting);
            // You can add more fields to update as necessary

            // Save changes
            try
            {
                await _ServiceSphereContext.SaveChangesAsync();
                return Ok("Service posting updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the service posting: {ex.Message}");
            }
        }

        [HttpDelete("ServicePosting/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteServicePosting(int id)
        {
            // Find the service posting by ID
            var servicePosting = await _servicePostingRepo.GetByIdAsync(id);
            if (servicePosting == null)
            {
                return NotFound($"Service posting with ID {id} not found.");
            }

            // Optional: Check if the current user is authorized to delete this service posting
            // This step depends on your application's requirements
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _userManager.FindByEmailAsync(Email);
            if (currentUser == null || servicePosting.userID != currentUser.Id)
            {
                // Not found response is used to avoid disclosing information about the existence of the resource
                return NotFound("No matching service posting found or you are not authorized to delete it.");
            }
            // Find dependent proposals and delete or handle them
            var proposals = await _ServiceSphereContext.Proposals
                .Where(p => p.ServicePostingId == id)
                .ToListAsync();

            if (proposals.Any())
            {
                // Assuming you want to delete the proposals. If you need to handle them differently (e.g., archiving), adjust this part.
                _ServiceSphereContext.Proposals.RemoveRange(proposals);
            }
            // Delete the service posting
            _servicePostingRepo.Delete(servicePosting);

            // Save changes
            try
            {
                await _ServiceSphereContext.SaveChangesAsync();
                return Ok($"Service posting with ID {id} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                // Consider using a logging framework or service
                return StatusCode(500, $"An error occurred while deleting the service posting: {ex.Message}");
            }
        }



        [HttpGet("GetAllServicePostings")]
        public async Task<ActionResult<IEnumerable<ServicePostingDto>>> GetServicePostings([FromQuery] PostsSpecParams? @params)
        {
            var spec = new ServicePostingWithCategorySpec(@params);//hycall first ctor create obj of criteria, includes
            var ServicePostings = await _servicePostingRepo.GetAllWithSpecAsync(spec);
            // Retrieve all service postings from the database

            /*var servicePostings = await _ServiceSphereContext.ServicePostings
                .Include(sp => sp.Category) // Include category if needed
                .ToListAsync();*/

            // Map the retrieved service postings to DTOs

            var servicePostingsDto = ServicePostings.Select(sp => new ServicePostingDto
            {
                Id = sp.Id,
                Title = sp.Title,
                Description = sp.Description,
                CategoryId = sp.CategoryId,
                UserId = sp.userID,
                Budget = sp.Budget,
                Duration = sp.Duration,
                Deadline = sp.Deadline
                // Map other properties as needed
            });


            return Ok(servicePostingsDto);
        }



        [HttpPost("ProjectPosting")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ProjectPostingDto>> PostProject([FromBody] ProjectPostingDto model)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get current user
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }

            // Check if the user exists
            //var userExists = await _userManager.FindByIdAsync(model.UserId) != null;
            //if (!userExists)
            //{
            //    return NotFound($"User with ID {model.UserId} not found.");
            //}
            model.UserId = user.Id;

            //get clientid
            var Client = await _ServiceSphereContext.Clients.Where(C => C.Email == user.Email).FirstOrDefaultAsync();
            if (Client == null) return BadRequest(new ApiResponse(400, "Client can not be found"));
            model.ClientId = Client.Id;

            // map project posting
            var projectPosting = _mapper.Map<ProjectPosting>(model);

            // Assuming model includes something like List<SubCategoryRequirement> SelectedSubCategories
            // where SubCategoryRequirement might be a DTO with SubCategoryId and TeamMembersRequired properties
            var projectSubCategories = new List<ProjectSubCategory>();
            foreach (var sc in model.ProjectSubCategories) // Adjust according to your actual structure
            {
                var projectSubCategory = new ProjectSubCategory
                {
                    SubCategoryId = sc.SubCategoryId,
                    TeamMembersRequired = sc.TeamMembersRequired
                };
                projectSubCategories.Add(projectSubCategory);
            }

            projectPosting.ProjectSubCategories = projectSubCategories;

            // Remove assignment of identity column value if necessary
            // projectPosting.Id = 0; // Uncomment if 'Id' is set manually elsewhere

            // Add the project posting to the context
            _ServiceSphereContext.ProjectPostings.Add(projectPosting);

            // Save changes
            try
            {
                var mappedProjectPosts = _mapper.Map<ProjectPostingDto>(projectPosting);
                await _ServiceSphereContext.SaveChangesAsync();
                // Consider mapping back to DTO if necessary
                return Ok(mappedProjectPosts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }


        [HttpGet("ProjectPostings")]
        public async Task<ActionResult<IEnumerable<ProjectPostingDto>>> GetAllProjectPostings([FromQuery] PostsSpecParams? @params)
        {
            try
            {
                // Retrieve all project postings from the main database context
                //  var projectPostings = await _mainDbContext.ProjectPostings.ToListAsync();
                var spec = new ProjectPostingWithCategorySpec(@params);
                var projectPostings = await _projectPostingRepo.GetAllWithSpecAsync(spec);
                var mappedProjectPosts = _mapper.Map<IEnumerable<ProjectPostingDto>>(projectPostings);
                return Ok(mappedProjectPosts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving project postings: {ex.Message}");
            }
        }


        [HttpGet("GetServicePosting/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<GetServicePostingByIdDto>> GetServicePosting(int id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }

            var spec = new ServicePostingWithCategorySpec(id);
            var servicePosting = await _servicePostingRepo.GetByIdWithSpecAsync(spec);

            if (servicePosting == null)
            {
                return NotFound(new ApiResponse(404, "This Post doesn't exist"));
            }
            if (!(servicePosting.userID == user.Id))
            {
                return NotFound(new ApiResponse(404, "You don't have a post with this ID"));
            }


            var MappedServicePostings = _mapper.Map<GetServicePostingByIdDto>(servicePosting);
            // Optionally, load and map related user data from the UserManager if needed

            return Ok(MappedServicePostings);
        }





        [HttpGet("GetProjectPosting/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<GetProjectByIdDto>> GetProjectPosting(int id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }

            var spec = new ProjectPostingWithCategorySpec(id);
            var projectPosting = await _projectPostingRepo.GetByIdWithSpecAsync(spec);

            if (projectPosting == null)
            {
                return NotFound(new ApiResponse(404, "This Post doesn't exist"));
            }
            if (!(projectPosting.userID == user.Id))
            {
                return NotFound(new ApiResponse(404, "You don't have a post with this ID"));
            }

            var MappedProjectPostings = _mapper.Map<GetProjectByIdDto>(projectPosting);
            // Optionally, load and map related user data from the UserManager if needed

            return Ok(MappedProjectPostings);
        }


        [HttpPut("ProjectPosting/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateProjectPosting(int id, [FromBody] ProjectPostingDto model)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the service posting exists
            var projectPosting = await _projectPostingRepo.GetByIdAsync(id);
            if (projectPosting == null)
            {
                return NotFound($"project posting with ID {id} not found.");
            }
            model.Id = id;
            // Get current user
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _userManager.FindByEmailAsync(Email);
            if (currentUser == null)
            {
                return NotFound(new ApiResponse(404, "Current user not found."));
            }

            // Check if the current user is authorized to update the service posting
            if (projectPosting.userID != currentUser.Id)
            {
                return Forbid("You are not authorized to update this project posting.");
            }

            // Update service posting details
            _mapper.Map(model, projectPosting);
            // You can add more fields to update as necessary

            // Save changes
            try
            {
                await _ServiceSphereContext.SaveChangesAsync();
                return Ok("project posting updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the project posting: {ex.Message}");
            }
        }

        [HttpDelete("ProjectPosting/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteProjectPosting(int id)
        {
            // Find the service posting by ID
            var projectPosting = await _projectPostingRepo.GetByIdAsync(id);
            if (projectPosting == null)
            {
                return NotFound($"Project posting with ID {id} not found.");
            }

            // Optional: Check if the current user is authorized to delete this service posting
            // This step depends on your application's requirements
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await _userManager.FindByEmailAsync(Email);
            if (currentUser == null || projectPosting.userID != currentUser.Id)
            {
                // Not found response is used to avoid disclosing information about the existence of the resource
                return NotFound("No matching Project posting found or you are not authorized to delete it.");
            }
            // Find dependent proposals and delete or handle them
            var proposals = await _ServiceSphereContext.Proposals
                .Where(p => p.ProjectPostingId == id)
                .ToListAsync();

            if (proposals.Any())
            {
                // Assuming you want to delete the proposals. If you need to handle them differently (e.g., archiving), adjust this part.
                _ServiceSphereContext.Proposals.RemoveRange(proposals);
            }
            // Delete the service posting
            _projectPostingRepo.Delete(projectPosting);

            // Save changes
            try
            {
                await _ServiceSphereContext.SaveChangesAsync();
                return Ok($"Project posting with ID {id} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                // Consider using a logging framework or service
                return StatusCode(500, $"An error occurred while deleting the Project posting: {ex.Message}");
            }
        }
        //[HttpPut("CloseProjectPost")]
        //public async Task<IActionResult> CloseProjectPost(int projectId)
        //{
        //    var 
        //}

    }
}

