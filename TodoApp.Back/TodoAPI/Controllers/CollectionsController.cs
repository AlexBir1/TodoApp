using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Collections;
using TodoAPI.CQRS.Queries.Collections;
using TodoAPI.DAL.Entities;
using TodoAPI.Filters;
using TodoAPI.Hubs;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<NotificationHub> _notifier;

        public CollectionsController(IMediator mediator, IHubContext<NotificationHub> notifier)
        {
            _notifier = notifier;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Collection>>>> GetAllAsync([FromQuery] string accountId) => Ok(await _mediator.Send(new GetCollectionListByAccountIdQuery(accountId)));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Collection>>> DeleteAsync(string id)
        {
            var result = await _mediator.Send(new DeleteCollectionCommand(id));
            if (result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Collection is successfully deleted");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IAPIResponse<Collection>>> UpdateAsync(string id, [FromBody] CollectionModel model)
        {
            var result = await _mediator.Send(new UpdateCollectionCommand(id, model));
            if (result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Collection is successfully updated");
            return Ok(result);
        }

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<IAPIResponse<Collection>>> CreateAsync([FromBody] CollectionModel model) 
        {
            var result = await _mediator.Send(new CreateCollectionCommand(model));
            return Ok(result);
        }
    }
}
