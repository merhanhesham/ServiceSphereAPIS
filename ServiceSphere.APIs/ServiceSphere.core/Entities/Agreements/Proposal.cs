using ServiceSphere.core.Entities.Posting;
using ServiceSphere.core.Entities.Users.Freelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Entities.Agreements
{
    /*public enum ProposalStatus
    {
        UnderReview,
        Accepted,
        Rejected,
        Completed,
    }*/

    public class Proposal : BaseEntity
    {
        public string ProposedTimeframe { get; set; }
        //public ProposalStatus Status { get; set; }
        public decimal ProposedBudget { get; set; }

        public string CoverLetter { get; set; } // A brief introduction and the freelancer's understanding of the project

        public string WorkPlan { get; set; } // A brief outline of how the freelancer plans to approach the project
        public string Milestones { get; set; } // Proposed milestones and deliverables for the project
        // Optional fields depending on the project requirements
        public string Availability { get; set; } // Freelancer's availability (hours per week, specific times)
        public string Inquiries { get; set; }  // Any questions the freelancer has for the client about the project

        public bool IsAccepted { get; set; }
        //fk for PostId, FreelancerId


        public int? ProjectPostingId { get; set; }
        public ProjectPosting? ProjectPosting { get; set; }
        public int? ServicePostingId { get; set; }
        public ServicePosting? ServicePosting { get; set; }

        public string? userId { get; set; }
        public string? FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }

        //navigational property for contract 
        public Contract Contract { get; set; }

        //public int PostId { get; set; } // Assuming PostId is an integer
        //public Post Post { get; set; } // Navigation property
    }
}
