using ServiceSphere.core.Entities.Assessments;
using ServiceSphere.core.Entities.Posting;

namespace ServiceSphere.APIs.DTOs
{
    public class ClientProfileToReturnDto
    {
        public string Id { get; set; }
        public string? Bio { get; set; }
        public string? CompanyName { get; set; }



        //nav prop for contract
      //  public ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
        //nav prop for post
      //  public ICollection<ProjectPostingDto> ProjectPostings { get; set; } = new HashSet<ProjectPostingDto>();
      //  public ICollection<ServicePostingDto> ServicePostings { get; set; } = new HashSet<ServicePostingDto>();
        //nav prop for notification 
        //public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        //nav prop for review
        public ICollection<ReviewDto> Reviews { get; set; } = new HashSet<ReviewDto>();
    }
}
