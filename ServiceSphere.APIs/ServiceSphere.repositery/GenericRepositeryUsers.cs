using Microsoft.EntityFrameworkCore;
using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Specifications;
using ServiceSphere.core.SpecificationsForUsers;
using ServiceSphere.repositery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.repositery
{
    public class GenericRepositeryUsers<T> where T : User
    {
        private readonly ServiceSphereContext _serviceSphereContext;

        public GenericRepositeryUsers(ServiceSphereContext serviceSphereContext)
        {
            _serviceSphereContext = serviceSphereContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _serviceSphereContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecificationsUsers<T> Spec)
        {
            return await ApplySpec(Spec).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _serviceSphereContext.Set<T>().FindAsync(Id);
        }

        public async Task<T?> GetByIdWithSpecAsync(ISpecificationsUsers<T> Spec)
        {
            return await ApplySpec(Spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpec(ISpecificationsUsers<T> Spec)
        {
            return SpecificationEvaluatorUsers<T>.GetQuery(_serviceSphereContext.Set<T>(), Spec);
        }


        public async Task AddAsync(T item)
        {
            await _serviceSphereContext.Set<T>().AddAsync(item);
        }

        public async void Delete(T item)
        {
            _serviceSphereContext.Remove(item);
        }
        public void Update(T item)
        {
            _serviceSphereContext.Update(item);
        }

    }
}
