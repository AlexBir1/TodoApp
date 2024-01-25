using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Categories;
using TodoAPI.CQRS.Queries.Categories;
using TodoAPI.DAL.Entities;
using TodoAPI.Hubs;
using TodoAPI.Services.Implementations;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<NotificationHub> _notifier;

        public CategoriesController(IHubContext<NotificationHub> notifier, IMediator mediator)
        {
            _notifier = notifier;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Category>>>> GetAllAsync([FromQuery] string accountId) => Ok(await _mediator.Send(new GetCategoryListByAccountIdQuery(accountId)));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Category>>> DeleteAsync(string id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            if (result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Category is successfully deleted");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<IAPIResponse<Category>>> CreateAsync([FromBody] CategoryModel model)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(model));
            if (result.IsSuccess)
                await _notifier.Clients.User(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).SendAsync("Notify", "Category is successfully created");
            return Ok(result);
        }
    }
}
