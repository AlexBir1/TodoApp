using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TodoAPI.APIResponse.Implementations;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands;
using TodoAPI.CQRS.Commands.Goals;
using TodoAPI.CQRS.Queries;
using TodoAPI.CQRS.Queries.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Filters;
using TodoAPI.Hubs;
using TodoAPI.Notifications;
using TodoAPI.Notifications.Service;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoalsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notifier;
        private readonly IMediator _mediator;

        public GoalsController(IHubContext<NotificationHub> notifier, IMediator mediator)
        {
            _notifier = notifier;
            _mediator = mediator;
        }

        [HttpPatch("{id}/AddToCategory/{selectedCategoryId}")]
        public async Task<ActionResult<IAPIResponse<CategoryGoal>>> AddToCategory(string id, string selectedCategoryId) => Ok(await _mediator.Send(new AddGoalToCategoryCommand(id, selectedCategoryId)));

        [HttpPatch("{id}/RemoveFromCategory/{selectedCategoryId}")]
        public async Task<ActionResult<IAPIResponse<CategoryGoal>>> RemoveFromCategory(string id, string selectedCategoryId) => Ok(await _mediator.Send(new RemoveGoalFromCategoryCommand(id, selectedCategoryId)));

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<IAPIResponse<Goal>>> CreateAsync(GoalModel model)
        {
            var result = await _mediator.Send(new CreateGoalCommand(model));
            if (result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Goal is successfully created");
            return Ok(result);
        }

        [ValidationFilter]
        [HttpPut("{id}")]
        public async Task<ActionResult<IAPIResponse<Goal>>> UpdateAsync(string id, GoalModel model) 
        {
            var result = await _mediator.Send(new UpdateGoalCommand(id, model));
            if (result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Goal is successfully updated");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Goal>>> DeleteAsync(string id)
        {
            var result = await _mediator.Send(new DeleteGoalCommand(id));
            if(result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Goal is successfully deleted");
            return Ok(result); 
        }

        [HttpGet]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Goal>>>> GetAllAsync
            ([FromQuery] string collectionId = "", [FromQuery] string searchQuery = "", [FromQuery] int itemsPerPage = 1, [FromQuery] int selectedPage = 1)
        {
            return Ok(await _mediator.Send(new GetGoalListByCollectionIdPagedQuery(collectionId, searchQuery, selectedPage, itemsPerPage)));
        }
    }
}
