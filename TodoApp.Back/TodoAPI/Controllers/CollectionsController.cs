using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Filters;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionsController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public CollectionsController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Collection>>>> GetAllAsync() => Ok(await _serviceRepo.CollectionService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<IAPIResponse<Collection>>> GetByIdAsync(string id) => Ok(await _serviceRepo.CollectionService.GetByIdAsync(id));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Collection>>> DeleteAsync(string id) => Ok(await _serviceRepo.CollectionService.DeleteAsync(id));

        [HttpPut("{id}")]
        public async Task<ActionResult<IAPIResponse<Collection>>> UpdateAsync(string id, [FromBody] CollectionModel model) => Ok(await _serviceRepo.CollectionService.UpdateAsync(id, model));

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<IAPIResponse<Collection>>> CreateAsync([FromBody] CollectionModel model) => Ok(await _serviceRepo.CollectionService.CreateAsync(model));
    }
}
