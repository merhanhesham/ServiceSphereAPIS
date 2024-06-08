using ServiceSphere.core.Entities.Assessments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Specifications
{
    public class AllNotificationsSpec:BaseSpecification<Notification>
    {
        public AllNotificationsSpec(string userId) :base(n => n.UserId == userId)
        {
            SetOrderByDesc(n=>n.date);
        }
    }
}
