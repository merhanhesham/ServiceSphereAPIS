using ServiceSphere.core.Entities.Assessments;
using ServiceSphere.core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Specifications
{
    public class NotificationsSpec : BaseSpecification<Notification>
    {
        public NotificationsSpec(string userId) : base(n => n.UserId == userId && !n.IsRead)
        {
            SetOrderByDesc(n => n.date);
        }
  
    }
}
