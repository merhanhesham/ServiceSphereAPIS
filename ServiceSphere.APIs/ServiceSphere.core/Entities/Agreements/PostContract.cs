using ServiceSphere.core.Entities.Users.Freelancer;
using ServiceSphere.core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Entities.Agreements
{
    public class PostContract:BaseEntity
    {
       
        public ContractStatus Status { get; set; } = ContractStatus.pending;
        public string Timeframe { get; set; }
        public decimal Budget { get; set; }
        public string CoverLetter { get; set; } // A brief introduction and the freelancer's understanding of the project
        public string WorkPlan { get; set; } // A brief outline of how the freelancer plans to approach the project
        public string Milestones { get; set; } // Proposed milestones and deliverables for the project
        // Optional fields depending on the project requirements
        public string Availability { get; set; }
        public string? ClientId { get; set; }
        public Client Client { get; set; }
        public string? FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
        public int? ProposalId { get; set; }
        public Proposal? Proposal { get; set; }
    }
}
