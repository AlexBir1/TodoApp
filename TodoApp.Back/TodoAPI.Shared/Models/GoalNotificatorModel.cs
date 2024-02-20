using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.Shared.Models
{
    public class GoalNotificatorModel
    {
        public Guid GoalId { get; set; } = Guid.NewGuid();
        public string JobId { get; set; }
        public DateTime GoalStartsAt { get; set; }

        public GoalNotificatorModel(string goalId, string jobId)
        {
            GoalId = Guid.Parse(goalId);
            JobId = jobId;
        }
    }
}
