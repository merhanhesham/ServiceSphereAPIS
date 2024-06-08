using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ServiceSphere.APIs.DTOs;
using ServiceSphere.APIs.Errors;
using ServiceSphere.core.Entities.Assessments;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.repositery.Data;
using ServiceSphere.services;
using System.Security.Claims;


namespace ServiceSphere.APIs.Controllers
{

    public class AssessmentsController : BaseController
    {
        private readonly NotificationService _notificationService;
        private readonly ServiceSphereContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IGenericRepositery<Notification> _notificationsRepo;
        private readonly ILogger<AssessmentsController> _logger;

        public AssessmentsController(NotificationService notificationService, ServiceSphereContext context, UserManager<AppUser>userManager, IHubContext<NotificationHub> hubContext , IGenericRepositery<Notification> NotificationsRepo, ILogger<AssessmentsController> logger)
        {
            _notificationService = notificationService;
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _notificationsRepo = NotificationsRepo;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost ("SendNotificationToFreelancer")]
        public async Task<IActionResult> SendNotificationToFreelancer([FromBody] CreateNotificationRequest request)
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(Email);
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            /*if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }*/
           // var Freelancer = await _context.Freelancers.Where(F => F.Email == user.Email).FirstOrDefaultAsync();
            //var freelancerId = Freelancer.Id;

            var notification = new Notification
            {
                UserId = userId,
                Message = request.Message,
                date = DateTime.Now
            };


            await _notificationsRepo.AddAsync(notification);
            await _context.SaveChangesAsync();

            // await _notificationService.CreateNotificationAsync(freelancerId, request.Message);
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", request.Message);

            //await NotificationHub.SendNotificationToUser(_hubContext, request.UserId.ToString(), request.Message);

            return Ok();
        }




        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("SendNotificationToUser")]
        public async Task<IActionResult> SendNotificationToUser([FromBody] CreateNotificationRequest request)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
          
            var notification = new Notification
            {
                UserId = userId,
                Message = request.Message,
                date = DateTime.Now
            };

            await _notificationsRepo.AddAsync(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessage", request.Message);
        

            return Ok($"notification sent to user of {userId}.");
        }










        [HttpGet ("GetUnreadNotifications")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUnreadNotifications()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var notifications = await _notificationService.GetUnreadNotificationsAsync(user.Id);
            await _notificationService.SetNotificationReadAsync(user.Id);
            return Ok(notifications);
        }
        [HttpGet("GetAllNotifications")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAllNotifications()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var notifications = await _notificationService.GetAllNotificationsAsync(user.Id);
            await _notificationService.SetNotificationReadAsync(user.Id);
            return Ok(notifications);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("PostReview")]
        public async Task<IActionResult> PostReview([FromBody] ReviewDto reviewDto, string targetUserId)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);


            var targetUser = targetUserId;
            if (targetUser == null)
            {
                return NotFound(new ApiResponse(404,"Target user not found.")); 
                //return NotFound("Target user not found.");

            }
            var review = new Review
            {
                Description = reviewDto.Description,
                TargetUserId = targetUser,
                Rating = reviewDto.Rating,
                ReviewerId = user.Id, // The current user is the reviewer
                ReviewerName=user.DisplayName,
                Date = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Review submitted successfully" });
        }

        [HttpGet("GetReviewsForUser")]
        public async Task<IActionResult> GetReviewsForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            //var Email = User.FindFirstValue(ClaimTypes.Email);

            // Check if the user exists
            //var freelancer = await _serviceSphereContext.Freelancers.Where(F => F.Email == Email).FirstOrDefaultAsync();
            
            var userExists = await _context.Freelancers.FindAsync(userId) != null;
            if (!userExists)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            // Retrieve all reviews for the specified user
            var reviews = await _context.Reviews
                .Where(r => r.TargetUserId == userId)
                .ToListAsync();

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound($"No reviews found for user with ID {userId}.");
            }

            return Ok(reviews);
        }



    }

    public class CreateNotificationRequest
    {
        //public string FreelancerId { get; set; }
        //public string ClientId { get; set; }
        //public string UserId { get; set; }
        public string Message { get; set; }
    }

    

}
