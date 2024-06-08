using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Users.Freelancer;
using ServiceSphere.core.Entities.Users;

namespace ServiceSphere.APIs.DTOs
{
    public class ContractDto
    {
        public string Terms { get; set; }
        public decimal Price { get; set; }
        //fk for clientid, proposalid

        public string? ClientId { get; set; }
        
        public string? FreelancerId { get; set; }
       
        public int? ProposalId { get; set; }
        public string? ServiceDetails { get; set; }

    }
}
