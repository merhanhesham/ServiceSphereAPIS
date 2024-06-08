using ServiceSphere.core.Entities.Posting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Specifications
{
    public class ProjectPostingWithCategorySpec:BaseSpecification<ProjectPosting>
    {
        public ProjectPostingWithCategorySpec(PostsSpecParams @params) : base(p =>
            (!@params.CategoryId.HasValue || p.CategoryId == @params.CategoryId) &&
         (string.IsNullOrEmpty(@params.userID) || p.userID == @params.userID)&&p.Status==PostStatus.Open)
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Team);
            Includes.Add(c => c.ProjectSubCategories);
            Includes.Add(c => c.Proposals);

        }
        public ProjectPostingWithCategorySpec(int PostID) : base(p =>
         (p.Id == PostID))
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Team);
            Includes.Add(c => c.ProjectSubCategories);
            Includes.Add(c => c.Proposals);
        }
    }
}
