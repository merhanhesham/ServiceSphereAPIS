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
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.core.Services.contract;
using ServiceSphere.core.Specifications;
using ServiceSphere.repositery.Data;
using ServiceSphere.repositery.Data.Migrations;
using System.Security.Claims;
using static ServiceSphere.core.Specifications.ProposalSpecs;

namespace ServiceSphere.APIs.Controllers
{
    
    public class AgreementsController : BaseController
    {
        private readonly IGenericRepositery<Proposal> _proposalRepo;
        private readonly ServiceSphereContext _serviceSphereContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepositery<ServicePosting> _servicePostingsRepo;
        private readonly INotificationService _notificationService;
        private readonly IGenericRepositery<Contract> _contractRepo;
        private readonly IGenericRepositery<PostContract> _postContractRepo;

        public AgreementsController(IGenericRepositery<Proposal> ProposalRepo, ServiceSphereContext serviceSphereContext, IMapper mapper, UserManager<AppUser>userManager, IGenericRepositery<ServicePosting>servicePostingsRepo, INotificationService notificationService ,IGenericRepositery<Contract> contractRepo,IGenericRepositery<PostContract> postContractRepo)
        {
            _proposalRepo = ProposalRepo;
            _serviceSphereContext = serviceSphereContext;
            _mapper = mapper;
            _userManager = userManager;
            _servicePostingsRepo = servicePostingsRepo;
            _notificationService = notificationService;
           _contractRepo = contractRepo;
            _postContractRepo = postContractRepo;
        }

        [HttpPost("SubmitProposalForProjectPosting")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SubmitProposalForProjectPosting([FromBody] ProposalDto proposalDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            //if(proposalDto.userId!=user.Id)
            //{
            //    return Unauthorized(new ApiResponse(404, "You Mustn't submit a proposal"));
            //}

            proposalDto.userId = user.Id;

            //set freelancer id 
            var freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == Email).FirstOrDefaultAsync();
            if (freelancer == null) return BadRequest(new ApiResponse(404, "No freelancer found"));
            proposalDto.FreelancerId = freelancer.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if the ProjectPosting exists
            var projectPosting = await _serviceSphereContext.ProjectPostings.FindAsync(proposalDto.ProjectPostingId);
            if (projectPosting == null)
            {
                return NotFound($"ProjectPosting with ID {proposalDto.ProjectPostingId} not found.");
            }

            // Check if the user has already submitted a proposal for this project


            try
            {
                if (projectPosting.Status == PostStatus.Open)
                {
                    bool alreadySubmitted = await _serviceSphereContext.Proposals.AnyAsync(p => p.ProjectPostingId == proposalDto.ProjectPostingId && p.userId == user.Id);
                    if (alreadySubmitted)
                    {
                        return BadRequest("You have already submitted a proposal for this project.");
                    }
                    var mappedProposal = _mapper.Map<Proposal>(proposalDto);
                    await _proposalRepo.AddAsync(mappedProposal);
                    var result = await _serviceSphereContext.SaveChangesAsync();
                    if (result <= 0) { return null; }
                    return Ok(proposalDto);
                }
                else
                {
                    return BadRequest("This post is Closed, You can't submit a proposal");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }

        [HttpPost("SubmitProposalForServicePosting")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SubmitProposalForServicePosting([FromBody] ProposalDto proposalDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            //if (proposalDto.userId != user.Id)
            //{
            //    return Unauthorized(new ApiResponse(404, "You aren't authorized submit a proposal"));
            //}
            proposalDto.userId = user.Id;
            //set freelancer id 
            var freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == Email).FirstOrDefaultAsync();
            if (freelancer == null) return BadRequest(new ApiResponse(404, "No freelancer found"));
            proposalDto.FreelancerId = freelancer.Id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if the ProjectPosting exists
            var ServicePosting = await _serviceSphereContext.ServicePostings.FindAsync(proposalDto.ServicePostingId);
            if (ServicePosting == null)
            {
                return NotFound($"ServicePosting with ID {proposalDto.ServicePostingId} not found.");
            }



            try
            {
                if (ServicePosting.Status == PostStatus.Open)
                {
                    // Check if the user has already submitted a proposal for this project
                    bool alreadySubmitted = await _serviceSphereContext.Proposals.AnyAsync(p => p.ServicePostingId == proposalDto.ServicePostingId && p.userId == user.Id);
                    if (alreadySubmitted)
                    {
                        return BadRequest("You have already submitted a proposal for this project.");
                    }
                    var mappedProposal = _mapper.Map<Proposal>(proposalDto);
                    await _proposalRepo.AddAsync(mappedProposal);
                    var result = await _serviceSphereContext.SaveChangesAsync();
                    if (result <= 0) { return null; }
                    return Ok(proposalDto);
                }
                else
                {
                    return BadRequest("This post is Closed, You can't submit a proposal");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }

        [HttpGet("GetUserProposals")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserProposals()
        {
            
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(400, "user not recognized"));
            }
            var spec = new ProposalSpecs(user.Id);
            var proposals = await _proposalRepo.GetAllWithSpecAsync(spec);
            var mappedProposals = _mapper.Map<List<ProposalDto>>(proposals);


            if (proposals == null || !proposals.Any())
            {
                
                return NotFound(new ApiResponse(404, "No proposals found for this user."));
            }

            // You might want to map these entities to DTOs before returning them
            return Ok(mappedProposals);
        }

        

        [HttpPut("UpdateProposal/{PostId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateProposal([FromBody] UpdateProposalDto UpdateproposalDto, int PostId, [FromQuery] PostingType postingType)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
           

            var spec = new ProposalSpecs(PostId,postingType);
            var proposal = await _proposalRepo.GetByIdWithSpecAsync(spec);
            if (proposal == null)
            {
                return NotFound($"Proposal doesn't exist");
            }
            UpdateproposalDto.Id = proposal.Id;
            if (proposal.userId != user.Id)
            {
                return Unauthorized(new ApiResponse(400,"You do not have permission to update this proposal."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the changes from proposalDto to the tracked entity directly
                _mapper.Map(UpdateproposalDto, proposal); // This avoids creating a new instance

                // _proposalRepo.Update(proposal); // This line might be unnecessary if _mapper.Map updates the tracked entity
                var result = await _serviceSphereContext.SaveChangesAsync();
                if (result <= 0) { return null; }
                return Ok(UpdateproposalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the project: {ex.Message}");
            }
        }

        [HttpDelete("{proposalId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteProposal(int proposalId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "user not found."));
            }

            var proposal = await _proposalRepo.GetByIdAsync(proposalId);
            if (proposal == null)
            {
                return NotFound("Proposal not found.");
            }

            // Check if the user is authorized to delete the proposal
            // This is just an example; adjust the condition according to your authorization logic
            if (proposal.userId != user.Id )
            {
                return Unauthorized(new ApiResponse(400, "You do not have permission to delete this proposal."));
            }

            try
            {
                // Perform the deletion
                _proposalRepo.Delete(proposal);
                await _serviceSphereContext.SaveChangesAsync();
                return Ok("You deleted the proposal successfully");
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting: {ex.Message}");
            }
        }

        [HttpPatch("AcceptProposal/{proposalId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AcceptProposal(int proposalId)
        {
            var proposal = await _proposalRepo.GetByIdAsync(proposalId);
            if (proposal == null)
            {
                return NotFound("Proposal not found.");
            }

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "user not found."));
            }

            //if (!(user.Id == proposal.userId))
            //{
            //    return Unauthorized(new ApiResponse(400, "You do not have permission to accept this proposal."));

            //}

            // Accept the proposal
            proposal.IsAccepted = true;
            _proposalRepo.Update(proposal);
            await _serviceSphereContext.SaveChangesAsync();

            //send notification
            await _notificationService.CreateNotificationAsync(proposal.userId,"Congrats, Your proposal is accepted");
            
            //update serviceposting

            if(proposal.ServicePostingId!=null)
            {
                int proposalServicePost =(int) proposal.ServicePostingId;
                var servicePost =await _servicePostingsRepo.GetByIdAsync(proposalServicePost);
                servicePost.Status = PostStatus.Closed;
                _servicePostingsRepo.Update(servicePost);
                
            }
            await _serviceSphereContext.SaveChangesAsync();
            //update projectposting

            return Ok("Proposal accepted successfully.");
        }

        [HttpGet("ProposalForSerivePost")]
        public async Task<IActionResult> GetProposalForServicePost(int ServicePostId)
        {
            var Proposal=await _serviceSphereContext.Proposals.Where(P=>P.ServicePostingId==ServicePostId&&P.IsAccepted==false).ToListAsync();
            var ServicePost = await _servicePostingsRepo.GetByIdAsync(ServicePostId);
           
          await  _serviceSphereContext.SaveChangesAsync();
            var MappedProposals = _mapper.Map<List<ProposalDto>>(Proposal);
            return Ok(MappedProposals);
        }
        [HttpGet("ProposalForProjectPost")]
        public async Task<IActionResult> GetProposalForProjectPost(int ProjectPostId)
        {
            var Proposal = await _serviceSphereContext.Proposals.Where(P => P.ProjectPostingId == ProjectPostId&&P.IsAccepted==false).ToListAsync();
            var MappedProposals = _mapper.Map<List<ProposalDto>>(Proposal);
            return Ok(MappedProposals);
        }

        //=========================================Contracts================================================
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("MakeContract")]
        public async Task<IActionResult> MakeContract(ContractDto contractDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var client = await _serviceSphereContext.Clients.Where(F => F.Email == Email).FirstOrDefaultAsync();
            if (client == null) return BadRequest(new ApiResponse(404, "No client found"));
            var contract = _mapper.Map<Contract>(contractDto);
            contract.ClientId = client.Id;
           await _contractRepo.AddAsync(contract);
           await _serviceSphereContext.SaveChangesAsync();
            return Ok(contractDto);
        }

        //=======================================Post Contract==================================================
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("MakePostContract")]
        public async Task<IActionResult> MakeContractForPost(int ProposalId)
        {
            var proposal=await _serviceSphereContext.Proposals.Where(P=>P.Id==ProposalId).FirstOrDefaultAsync();
            if (proposal == null) return NotFound(new ApiResponse(404, "Not found Proposal"));
            PostContract postContract = new PostContract();
            postContract.CoverLetter = proposal.CoverLetter;
            postContract.WorkPlan= proposal.WorkPlan;
            postContract.Budget=proposal.ProposedBudget;
            postContract.Availability=proposal.Availability;
            postContract.Milestones=proposal.Milestones;
            postContract.ProposalId = proposal.Id;
            postContract.FreelancerId = proposal.FreelancerId;
            postContract.Timeframe = proposal.ProposedTimeframe;


            //get client id
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var client = await _serviceSphereContext.Clients.Where(F => F.Email == Email).FirstOrDefaultAsync();
            if (client == null) return BadRequest(new ApiResponse(404, "No client found"));
            postContract.ClientId = client.Id;

            //get post contract
            await _postContractRepo.AddAsync(postContract);
           await _serviceSphereContext.SaveChangesAsync();

            var contract = _mapper.Map<PostContractDto>(postContract);
            
            return Ok(contract);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetPostContracts")]
        public async Task<IActionResult> GetAllContractsForClient(int Status)
        {
            //get client id
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var client = await _serviceSphereContext.Clients.Where(F => F.Email == Email).FirstOrDefaultAsync();
            if (client == null) return BadRequest(new ApiResponse(404, "No client found"));
            var AllConracts = await _serviceSphereContext.PostContracts.Where(C => C.ClientId == client.Id&&C.Status==(ContractStatus)Status).ToListAsync();
            if (AllConracts == null) return NotFound(new ApiResponse(404, "Not found Contracts"));

            return Ok(_mapper.Map<IReadOnlyList<PostContract>>(AllConracts));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetTerminatedPostContracts")]
        public async Task<IActionResult> GetAllTerminatedContractsForClient()
        {
            //get client id
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var client = await _serviceSphereContext.Clients.Where(F => F.Email == Email).FirstOrDefaultAsync();
            if (client == null) return BadRequest(new ApiResponse(404, "No client found"));
            var AllConracts = await _serviceSphereContext.PostContracts.Where(C => C.ClientId == client.Id && C.Status == ContractStatus.Terminated).ToListAsync();
            if (AllConracts == null) return NotFound(new ApiResponse(404, "Not found Contracts"));

            return Ok(_mapper.Map<IReadOnlyList<PostContract>>(AllConracts));
        }
    }

}
