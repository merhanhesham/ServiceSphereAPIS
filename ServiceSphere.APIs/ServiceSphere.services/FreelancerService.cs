using Microsoft.AspNetCore.Identity;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Users.Freelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.services
{
    public class FreelancerService
    {
        private readonly UserManager<AppUser> _userManager;

        //public FreelancerService(UserManager<AppUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        //public Task<Freelancer> AddFreelancer(AppUser user)
        //{
        //    //            var usersInFreelancerRole = await _userManager.GetUsersInRoleAsync("Freelancer");
        //    if(user.rol)
        //}
    }
}
