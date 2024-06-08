using Microsoft.EntityFrameworkCore;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Specifications;
using ServiceSphere.core.SpecificationsForUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.repositery
{
    public static class SpecificationEvaluatorUsers<T> where T : User
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecificationsUsers<T> Spec)
        {
            var Query = InputQuery;

            //where
            if (Spec.Critria is not null)
            {
                Query = Query.Where(Spec.Critria);
            }
            //include
            if (Spec.Includes is not null)
            {
                Query = Spec.Includes.Aggregate(Query, (currentQuery, includeExp) => (currentQuery.Include(includeExp)));
            }
            if (Spec.OrderByDesc is not null)
            {
                Query = Query.OrderByDescending(Spec.OrderByDesc);
            }

            return Query;
        }
    }
}
