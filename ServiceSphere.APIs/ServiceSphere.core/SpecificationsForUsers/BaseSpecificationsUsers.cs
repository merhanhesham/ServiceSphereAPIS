using ServiceSphere.core.Entities;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.SpecificationsForUsers
{
    public class BaseSpecificationsUsers<T> : ISpecificationsUsers<T> where T : User
    {
        public Expression<Func<T, bool>> Critria { get; set ; }
        public List<Expression<Func<T, object>>> Includes { get ; set ; }
        public Expression<Func<T, object>> OrderByDesc { get ; set ; }

        public BaseSpecificationsUsers()
        {

        }
        public BaseSpecificationsUsers(Expression<Func<T, bool>> critriaExp)
        {
            Critria = critriaExp;
        }
        public void SetOrderByDesc(Expression<Func<T, object>> OrderByDesc)
        {
            this.OrderByDesc = OrderByDesc;
        }
    }
}
