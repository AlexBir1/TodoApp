using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TodoAPI.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var accountId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, accountId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await base.OnDisconnectedAsync(ex);
        }
    }
}
