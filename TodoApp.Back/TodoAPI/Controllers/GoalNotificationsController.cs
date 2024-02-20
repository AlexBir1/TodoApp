using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Notifications;
using TodoAPI.Notifications.Service;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoalNotificationsController : ControllerBase
    {
        private readonly IGoalNotificatorService _notifierService;
        private readonly GoalScheduler _scheduler;

        public GoalNotificationsController(IGoalNotificatorService notifierService, GoalScheduler scheduler)
        {
            _notifierService = notifierService;
            _scheduler = scheduler;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] IEnumerable<Goal> goals)
        {
            foreach (var g in goals)
            {
                var triggerResult = await _notifierService.GetByIdAsync(g.Id.ToString()) as APIResponse<GoalNotificator>;
                if (g.StartDate.AddMinutes(-10) > DateTime.Now && triggerResult.IsSuccess && triggerResult.Data == null)
                {
                    string newJobId = _scheduler.PrepareNotification(new UserNotification { GoalId = g.Id.ToString(), UserId = g.Collection.AccountId, NotifyAt = g.StartDate, Message = $"{g.Title} atarts in minutes" });
                    await _notifierService.CreateAsync(new GoalNotificatorModel(g.Id.ToString(), newJobId));
                }
            }

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateAsync(string id, Goal goal) 
        {
            var result = await _notifierService.GetByIdAsync(id) as APIResponse<GoalNotificator>;
            if (result.IsSuccess )
            {
                if (result.Data == null)
                {
                    string newJobId = _scheduler.PrepareNotification(new UserNotification { GoalId = goal.Id.ToString(), UserId = goal.Collection.AccountId, NotifyAt = goal.StartDate, Message = $"{goal.Title} atarts in minutes" });
                    await _notifierService.CreateAsync(new GoalNotificatorModel(goal.Id.ToString(), newJobId));

                    return Ok();
                }

                _scheduler.UpdateNotification(result.Data.JobId, goal.StartDate.AddMinutes(-10));
            }
                
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var result = await _notifierService.DeleteAsync(id) as APIResponse<GoalNotificator>;
            if (result.IsSuccess)
            {
                _scheduler.DeleteNotification(result.Data.JobId);
            }
            return Ok();
        }
    }
}
