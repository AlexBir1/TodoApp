using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentsController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public AttachmentsController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet("{goalId}")]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Attachment>>>> GetAllByGoalIdAsync(string goalId) => Ok(await _serviceRepo.AttachmentService.GetAllByGoalIdAsync(goalId));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Attachment>>> DeleteAsync(string id) => Ok(await _serviceRepo.AttachmentService.DeleteAsync(id));

        [HttpGet("{id}/Save")]
        public async Task<ActionResult> SaveAsync(string id)
        {
            var fileModel = await _serviceRepo.AttachmentService.DownloadAsync(id);
            return File(fileModel.File, fileModel.ContentType, fileModel.Filename);
        }

        [HttpPost("{goalId}")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        public async Task<ActionResult<IAPIResponse<Attachment>>> CreateAsync(string goalId) => Ok(await _serviceRepo.AttachmentService.CreateAsync(new AttachmentModel
        {
            GoalId = Guid.Parse(goalId),
            File = Request.Form.Files[0],
        }));
    }
}
