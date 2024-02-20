using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.Notifications
{
    public class UserNotification
    {
        public DateTime NotifyAt { get; set; }
        public string GoalId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}
