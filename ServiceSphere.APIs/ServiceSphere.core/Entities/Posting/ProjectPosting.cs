using ServiceSphere.core.Entities.Agreements;
using ServiceSphere.core.Entities.Services;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Entities.Users.Freelancer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Entities.Posting
{
    public class ProjectPosting : BaseEntity
    {

        //post
        public string? Title { get; set; }
        public string Description { get; set; }

        public PostStatus Status { get; set; }
        public decimal? Budget { get; set; }

        public string? Duration { get; set; }

        public DateTime? Deadline { get; set; }

        //foreign key for the client 
        //public string? ClientId { get; set; }
        //public Client Client { get; set; }
        public string userID { get; set; }

        //foreign key for the category
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        // public ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
        // nav prop for proposals
        public ICollection<Proposal> Proposals { get; set; } = new HashSet<Proposal>();
        public Team Team { get; set; }

        // Replace the ICollection<SubCategory> with ICollection<ProjectSubCategory>
        public ICollection<ProjectSubCategory> ProjectSubCategories { get; set; } = new HashSet<ProjectSubCategory>();

        public string ClientId { get; set; }
    }
}


