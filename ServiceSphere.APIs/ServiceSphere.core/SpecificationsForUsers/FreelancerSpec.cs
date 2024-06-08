using ServiceSphere.core.Entities.Users.Freelancer;
using ServiceSphere.core.SpecificationsForUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.SpecificationsForUsers
{
    public class FreelancerSpec:BaseSpecificationsUsers<Freelancer>
    {
        public FreelancerSpec(FreelancerSpecParams freelancerParams) : base(f => (string.IsNullOrEmpty(freelancerParams.search) ||
        f.DisplayName.ToLower().Contains(freelancerParams.search.ToLower())) && 
        (!freelancerParams.CategoryId.HasValue || f.Categories.Any(c => c.Id == freelancerParams.CategoryId)) && 
        (!freelancerParams.SubCategoryId.HasValue || f.SubCategories.Any(sc => sc.Id == freelancerParams.SubCategoryId)))
        {
            Includes.Add(F => F.Categories);
            Includes.Add(F => F.SubCategories);
            Includes.Add(F => F.Skills);
            Includes.Add(F => F.Reviews);
            Includes.Add(F => F.Proposals);
            Includes.Add(F => F.Services);

        }
    }
}
