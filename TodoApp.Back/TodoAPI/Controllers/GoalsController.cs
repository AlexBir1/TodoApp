using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Filters;
using TodoAPI.Services.Interfaces;
using TodoAPI.Services.UOW;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoalsController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public GoalsController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpPatch("{id}/AddToCategory/{selectedCategoryId}")]
        public async Task<ActionResult<IAPIResponse<CategoryGoal>>> AddToCategory(string id, string selectedCategoryId) => Ok(await _serviceRepo.GoalService.AddToCategory(id, selectedCategoryId));

        [HttpPatch("{id}/RemoveFromCategory/{selectedCategoryId}")]
        public async Task<ActionResult<IAPIResponse<CategoryGoal>>> RemoveFromCategory(string id, string selectedCategoryId) => Ok(await _serviceRepo.GoalService.RemoveFromCategory(id, selectedCategoryId));

        [ValidationFilter]
        [HttpPost]
        public async Task<ActionResult<IAPIResponse<Goal>>> CreateAsync(GoalModel model) => Ok(await _serviceRepo.GoalService.CreateAsync(model));

        [ValidationFilter]
        [HttpPut("{id}")]
        public async Task<ActionResult<IAPIResponse<Goal>>> UpdateAsync(string id, GoalModel model) => Ok(await _serviceRepo.GoalService.UpdateAsync(id, model));

        [HttpGet("{id}")]
        public async Task<ActionResult<IAPIResponse<Goal>>> GetByIdAsync(string id) => Ok(await _serviceRepo.GoalService.GetByIdAsync(id));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Goal>>> DeleteAsync(string id) => Ok(await _serviceRepo.GoalService.DeleteAsync(id));

        [HttpGet]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Goal>>>> GetAllAsync([FromQuery] string collectionId = "", [FromQuery] string searchQuery = "")
        {
            if(!string.IsNullOrEmpty(searchQuery))
                return Ok(await _serviceRepo.GoalService.GetAllAsync(x => x.Title.Contains(searchQuery) || x.Description.Contains(searchQuery) || x.GoalCategories.Any(x=>x.Category.ColorTitle.Contains(searchQuery))));

            if (string.IsNullOrEmpty(collectionId))
                return Ok(await _serviceRepo.GoalService.GetAllAsync());
            else
                return Ok(await _serviceRepo.GoalService.GetAllAsync(x => x.CollectionId == Guid.Parse(collectionId)));
        }
    }
}
