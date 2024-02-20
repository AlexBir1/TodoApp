using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.DAL.Entities
{
    public class GoalNotificator
    {
        public Guid GoalId { get; set; } = Guid.NewGuid();

        public string JobId { get; set; }

        public Goal Goal { get; set; }
    }
}
