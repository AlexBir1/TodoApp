using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public CategoriesController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Category>>>> GetAllAsync([FromQuery] string accountId) => Ok(await _serviceRepo.CategoryService.GetAllAsync(x=>x.AccountId == accountId));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Category>>> DeleteAsync(string id) => Ok(await _serviceRepo.CategoryService.DeleteAsync(id)); 

        [HttpPost]
        public async Task<ActionResult<IAPIResponse<Category>>> CreateAsync([FromBody] CategoryModel model) => Ok(await _serviceRepo.CategoryService.CreateAsync(model));
    }
}
