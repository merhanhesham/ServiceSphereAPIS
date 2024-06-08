using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ServiceSphere.services
{
    public class NotificationHub : Hub
    {
        /*public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Assume you set this somewhere during authentication
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            return base.OnDisconnectedAsync(exception);
        }*/

        /*public async Task SendNotificationToUser(string userId, string message)
        {
            await Clients.Group(userId).SendAsync("ReceiveNotification", message);
        }*/

        public async Task SendMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", message);
        }

    }
}
