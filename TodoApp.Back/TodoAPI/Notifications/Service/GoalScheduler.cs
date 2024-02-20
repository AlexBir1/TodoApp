using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TodoAPI.Hubs;

namespace TodoAPI.Notifications.Service
{
    public class GoalScheduler
    {
        private readonly IHubContext<NotificationHub> notifier;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public GoalScheduler(IHubContext<NotificationHub> notifier, IBackgroundJobClient backgroundJobClient)
        {
            this.notifier = notifier;
            _backgroundJobClient = backgroundJobClient;
        }

        public string PrepareNotification(UserNotification notification)
        {
            return _backgroundJobClient.Schedule(() => Notify(notification), notification.NotifyAt);
        }

        public bool UpdateNotification(string jobId, DateTime newDateTime)
        {
            return _backgroundJobClient.Reschedule(jobId, newDateTime);
        }

        public bool DeleteNotification(string jobId)
        {
            bool result = _backgroundJobClient.Delete(jobId);
            return result;
        }

        public void Notify(UserNotification notification)
        {
            notifier.Clients.User(notification.UserId).SendAsync("Notify", notification);
        }
    }
}