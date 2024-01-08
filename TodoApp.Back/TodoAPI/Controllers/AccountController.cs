using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.Filters;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public AccountController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [ValidationFilter]
        [HttpPost("SignUp")]
        public async Task<ActionResult<IAPIResponse<AuthorizationModel>>> SignUpAsync([FromBody] SignUpModel model) => Ok(await _serviceRepo.AccountService.SignUpAsync(model));

        [ValidationFilter]
        [HttpPost("SignIn")]
        public async Task<ActionResult<IAPIResponse<AuthorizationModel>>> SignInAsync([FromBody] SignInModel model) => Ok(await _serviceRepo.AccountService.SignInAsync(model));

        [ValidationFilter]
        [HttpGet("{id}")]
        public async Task<ActionResult<IAPIResponse<AccountModel>>> GetByIdAsync([FromBody] string id) => Ok(await _serviceRepo.AccountService.GetByIdAsync(id));

        [HttpPut("RefreshAuthToken")]
        public async Task<ActionResult<IAPIResponse<AuthorizationModel>>> RefreshTokenAsync([FromBody] AuthorizationModel model) => Ok(await _serviceRepo.AccountService.RefreshTokenAsync(model));
    }
}
