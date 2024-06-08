using ServiceSphere.core.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Specifications
{
    public class ServiceSpec:BaseSpecification<Service>
    {
        public ServiceSpec(string userId):base(S=>S.FreelancerId == userId)
        {
            Includes.Add(S => S.Category);
        }
    }
}
