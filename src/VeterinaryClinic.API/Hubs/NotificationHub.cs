

using Microsoft.AspNetCore.SignalR;

namespace VeterinaryClinic.API.Hubs
{
   public class NotificationHub: Hub
    {
         private readonly ILogger<NotificationHub> _logger;
         public NotificationHub (ILogger<NotificationHub> logger)
        {
          _logger = logger;   
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            
            await Clients.Others.SendAsync("UserConnected", $"User {Context.ConnectionId} has connected");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            await Clients.Others.SendAsync("UserDisconnected", $"User {Context.ConnectionId} has disconnected");
            
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation($"User {Context.ConnectionId} has joined the group {groupName}");
            await Clients.Group(groupName).SendAsync("GroupNotification", $"User {Context.ConnectionId} has joined the group {groupName}");
            
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation($"User {Context.ConnectionId} has left the group {groupName}");
            await Clients.Group(groupName).SendAsync("GroupNotification", $"User {Context.ConnectionId} has left the group {groupName}");
        }  

        public async Task SendNotificationAll(string title, string message)
        {
            _logger.LogInformation($"Notification sent to all: {title} - {message}");
            await Clients.All.SendAsync("ReceiveNotification", new
            {
                Title = title,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                From = Context.ConnectionId

            });

        }

        public async Task SendNotificationToGroup(string groupName, string title, string message)
        {
            _logger.LogInformation($"Notification sent to group {groupName}: {title} - {message}");
            await Clients.Group(groupName).SendAsync("ReceiveNotification", new
            {
                Title = title,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                From = Context.ConnectionId
            });
        }

    }

}