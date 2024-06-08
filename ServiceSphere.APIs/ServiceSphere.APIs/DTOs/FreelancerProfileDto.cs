using ServiceSphere.core.Entities.Assessments;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Entities.Users.Freelancer;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSphere.APIs.DTOs
{
    public class FreelancerProfileDto
    {
       
        public string? PhoneNumber { get; set; }
        public string? Title { get; set; }
        public string? WorkExperience { get; set; }

        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? experienceLevel { get; set; }
        public string? Education { get; set; }
        //public string? Overview { get; set; }

        //public string? WorkStyle { get; set; }
        public List<Skill>? Skills { get; set; }
        //public string? UserId { get; set; }

        // public int? CategoryId { get; set; }

        public ICollection<int> CategoryIds { get; set; } = new HashSet<int>();
        public ICollection<int> SubCategoryIds { get; set; } = new HashSet<int>();

        //nav prop for proposal
        //public ICollection<Proposal> Proposals { get; set; }=new HashSet<Proposal>();
        //nav prop for category
        //  public ICollection<CategoryDto> Categories { get; set; } = new HashSet<CategoryDto>();
        // public ICollection<ServiceDto> Services { get; set; } = new HashSet<ServiceDto>();
        //nav prop for notification 
        //  public ICollection<NotificationDto> Notifications { get; set; } = new HashSet<NotificationDto>();
        //nav prop for review
        //  public ICollection<ReviewDto> Reviews { get; set; } = new HashSet<ReviewDto>();
    }
}
