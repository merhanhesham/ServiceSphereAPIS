using ServiceSphere.core.Entities.Posting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Entities.Services
{
    public class ProjectSubCategory
    {
        public int ProjectPostingId { get; set; }
        public ProjectPosting ProjectPosting { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public int TeamMembersRequired { get; set; } // Number of team members required for the subcategory
    }
}
