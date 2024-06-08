using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Entities.Users.Freelancer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Entities.Agreements
{
    public enum ContractStatus
    {
        pending,
        canceled, //client cancel the contract
        Active,  //client paid
        Completed, //work completed but freelancer is waiting for his money
        Terminated,
        // Add more status values as needed
    }
    public class Contract:BaseEntity
    {
        public string Terms { get; set; }
        public string ServiceDetails { get; set; }
        public ContractStatus Status { get; set; } = ContractStatus.pending;
        public decimal Price { get; set; }
        //fk for clientid, proposalid
        
        public string? ClientId { get; set; }
        public Client Client { get; set; }
        public string? FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
        public int? ProposalId { get; set; }
        public Proposal? Proposal { get; set; }
    }
}
