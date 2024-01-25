using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Account;
using TodoAPI.CQRS.Queries.Account;
using TodoAPI.Filters;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ValidationFilter]
        [HttpPost("SignUp")]
        public async Task<ActionResult<IAPIResponse<AuthorizationModel>>> SignUpAsync([FromBody] SignUpModel model) => Ok(await _mediator.Send(new SignUpAccountCommand(model)));

        [ValidationFilter]
        [HttpPost("SignIn")]
        public async Task<ActionResult<IAPIResponse<AuthorizationModel>>> SignInAsync([FromBody] SignInModel model) => Ok(await _mediator.Send(new SignInAccountCommand(model)));

        [ValidationFilter]
        [HttpGet("{id}")]
        public async Task<ActionResult<IAPIResponse<AccountModel>>> GetByIdAsync([FromBody] string id) => Ok(await _mediator.Send(new GetAccountByIdQuery(id)));

        [HttpPut("RefreshAuthToken")]
        public async Task<ActionResult<IAPIResponse<AuthorizationModel>>> RefreshTokenAsync([FromBody] AuthorizationModel model) => Ok(await _mediator.Send(new RefreshTokenAccountCommand(model)));
    }
}
