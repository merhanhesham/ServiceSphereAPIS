using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceSphere.core.Entities.Assessments;
using ServiceSphere.core.Entities.Users;
using ServiceSphere.core.Repositeries.contract;
using ServiceSphere.core.Services.contract;
using ServiceSphere.core.Specifications;
using ServiceSphere.repositery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSphere.services
{
    public class NotificationService:INotificationService
    {
        private readonly ServiceSphereContext _context;
        private readonly IGenericRepositery<Notification> _notificationsRepo;

        public NotificationService(ServiceSphereContext context, IGenericRepositery<Notification>NotificationsRepo)
        {
            _context = context;
            _notificationsRepo = NotificationsRepo;
        }

        public async Task CreateNotificationAsync(string userId, string message)
        {

            
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            /*return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();*/

            var spec = new NotificationsSpec(userId);
            return await _notificationsRepo.GetAllWithSpecAsync(spec);
        }
        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(string userId)
        {
            var spec = new AllNotificationsSpec(userId);
            return await _notificationsRepo.GetAllWithSpecAsync(spec);
        }
        public async Task<IEnumerable<Notification>> SetNotificationReadAsync(string userId)
        {
            var spec = new NotificationsSpec(userId);
            var Notifications = await _notificationsRepo.GetAllWithSpecAsync(spec);
            foreach (var notification in Notifications)
            {
                notification.IsRead = true;
                // Assuming you have an update method in your repository to persist changes
                _notificationsRepo.Update(notification);
               
            }
            await _context.SaveChangesAsync();
            return Notifications;
        }

    }
}


