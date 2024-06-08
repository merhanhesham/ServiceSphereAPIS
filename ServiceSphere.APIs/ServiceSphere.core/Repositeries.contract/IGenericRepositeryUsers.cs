using ServiceSphere.core.Entities.Identity;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Specifications;
using ServiceSphere.core.SpecificationsForUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Repositeries.contract
{
    public interface IGenericRepositeryUsers<T> where T : User
    {
        public Task<T?> GetByIdAsync(int Id);
        public Task<T?> GetByIdWithSpecAsync(ISpecificationsUsers<T> Spec);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecificationsUsers<T> Spec);

        public Task AddAsync(T item);
        public void Update(T item);
        public void Delete(T item);
    }
}
