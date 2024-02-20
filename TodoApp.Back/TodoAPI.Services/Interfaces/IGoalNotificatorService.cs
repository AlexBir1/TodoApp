using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Services.Interfaces
{
    public interface IGoalNotificatorService : IService<GoalNotificator, GoalNotificatorModel>
    {
    }
}
