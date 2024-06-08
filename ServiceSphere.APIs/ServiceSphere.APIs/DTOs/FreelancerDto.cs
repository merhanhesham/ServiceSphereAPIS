using ServiceSphere.core.Entities.Users.Freelancer;

namespace ServiceSphere.APIs.DTOs
{
    public class FreelancerDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //  public string PhoneNumber { get; set; }
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public ExperienceLevel? experienceLevel { get; set; }
      //  public string? Education { get; set; }
      //  public string? Overview { get; set; }

      //  public string? WorkStyle { get; set; }
        public List<Skill>? Skills { get; set; }
        //  public string UserId { get; set; }

        //nav prop for proposal
        //  public ICollection<Proposal> Proposals { get; set; } = new HashSet<Proposal>();

        //nav prop for category
        public ICollection<CategoryDto> Categories { get; set; } = new HashSet<CategoryDto>();
        public ICollection<ServiceDto> Services { get; set; } = new HashSet<ServiceDto>();
        //nav prop for notification 
        // public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        //nav prop for review
      //  public ICollection<ReviewDto> Reviews { get; set; } = new HashSet<ReviewDto>();

        public ICollection<SubCategoryDto>? SubCategories { get; set; } = new HashSet<SubCategoryDto>();
    }
}
