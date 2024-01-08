using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

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
        public async Task<ActionResult<IAPIResponse<IEnumerable<Category>>>> GetAllAsync() => Ok(await _serviceRepo.CategoryService.GetAllAsync());
    }
}
