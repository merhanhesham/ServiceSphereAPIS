using ServiceSphere.core.Entities.Posting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Specifications
{
    public class ServicePostingWithCategorySpec : BaseSpecification<ServicePosting>
    {
        public ServicePostingWithCategorySpec(PostsSpecParams @params) : base(p =>
         (!@params.CategoryId.HasValue || p.CategoryId == @params.CategoryId) &&
         (string.IsNullOrEmpty(@params.EmailAddress) || p.EmailAddress == @params.EmailAddress)&&p.Status==PostStatus.Open)
            {
                Includes.Add(c => c.Category);
            }

        public ServicePostingWithCategorySpec(int PostID) : base(p =>
         ( p.Id == PostID) )
        {
            Includes.Add(c => c.Category);
            Includes.Add(c => c.Proposals);
        }

    }
    /*public ServicePostingWithCategorySpec():base()
    {
        Includes.Add(c => c.Category);
    }*/
}

