namespace ServiceSphere.APIs.DTOs
{
    public class ProposalDto
    {
        public int Id { get; set; }
        public string ProposedTimeframe { get; set; }
        public decimal ProposedBudget { get; set; }
        public int? ProjectPostingId { get; set; }
        public int? ServicePostingId { get; set; }
        //public string FreelancerId { get; set; }
        // Add any other fields you expect from the request
        public string? userId { get; set; }
        public string CoverLetter { get; set; } // A brief introduction and the freelancer's understanding of the project

        public string WorkPlan { get; set; } // A brief outline of how the freelancer plans to approach the project
        public string Milestones { get; set; } // Proposed milestones and deliverables for the project
        // Optional fields depending on the project requirements
        public string Availability { get; set; } // Freelancer's availability (hours per week, specific times)
        public string Inquiries { get; set; } // Any questions the freelancer has for the client about the project
        public bool IsAccepted { get; set; } = false;
        public string? FreelancerId { get; set; }

    }

}
