using ServiceSphere.core.Entities.Posting;

namespace ServiceSphere.APIs.DTOs
{
    public class UpdateProposalDto
    {
        public int Id { get; set; }
        public string ProposedTimeframe { get; set; }
        //public ProposalStatus Status { get; set; }
        public decimal ProposedBudget { get; set; }

        public string CoverLetter { get; set; } // A brief introduction and the freelancer's understanding of the project

        public string WorkPlan { get; set; } // A brief outline of how the freelancer plans to approach the project
        public string Milestones { get; set; } // Proposed milestones and deliverables for the project
        // Optional fields depending on the project requirements
        public string Availability { get; set; } // Freelancer's availability (hours per week, specific times)
        public string Inquiries { get; set; }  // Any questions the freelancer has for the client about the project
    }
}
