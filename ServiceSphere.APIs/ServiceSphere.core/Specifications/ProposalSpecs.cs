using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Specifications
{
    public class ProposalSpecs:BaseSpecification<Proposal>
    {
        public ProposalSpecs(string userId):base(p => (p.ProjectPosting != null && p.ProjectPosting.userID == userId) ||
                            (p.ServicePosting != null && p.ServicePosting.userID == userId))
        {

            Includes.Add(p => p.ProjectPosting);
            Includes.Add(p => p.ServicePosting);

        }

        public enum PostingType
        {
            Service,
            Project
        }
        public ProposalSpecs(int postId, PostingType postingType)
    : base(p => postingType == PostingType.Service ? p.ServicePostingId == postId : p.ProjectPostingId == postId)
        {
        }

    }
}
