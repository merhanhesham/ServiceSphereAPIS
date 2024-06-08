using ServiceSphere.core.Entities.Assessments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.core.Services.contract
{
    public interface INotificationService
    {
        public Task CreateNotificationAsync(string userId, string message);
        public Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId);
        public Task<IEnumerable<Notification>> GetAllNotificationsAsync(string userId);

        public Task<IEnumerable<Notification>> SetNotificationReadAsync(string userId);

    }
}
