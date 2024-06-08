using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Users.Freelancer;
using ServiceSphere.core.Entities.Users;

namespace ServiceSphere.APIs.DTOs
{
    public class PostContractDto
    {
        public int Id { get; set; }
        public ContractStatus Status { get; set; }
        public string Timeframe { get; set; }
        public decimal Budget { get; set; }
        public string CoverLetter { get; set; } // A brief introduction and the freelancer's understanding of the project
        public string WorkPlan { get; set; } // A brief outline of how the freelancer plans to approach the project
        public string Milestones { get; set; } // Proposed milestones and deliverables for the project
        // Optional fields depending on the project requirements
        public string Availability { get; set; }
        public string? ClientId { get; set; }
        public string? FreelancerId { get; set; }
       
    }
}
