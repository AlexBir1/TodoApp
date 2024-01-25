using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Attachments;
using TodoAPI.CQRS.Queries.Attachments;
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
        private readonly IMediator _mediator;

        public AttachmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{goalId}")]
        public async Task<ActionResult<IAPIResponse<IEnumerable<Attachment>>>> GetAllByGoalIdAsync(string goalId) => Ok(await _mediator.Send(new GetAttachmentListByGoalIdQuery(goalId)));

        [HttpDelete("{id}")]
        public async Task<ActionResult<IAPIResponse<Attachment>>> DeleteAsync(string id) => Ok(await _mediator.Send(new DeleteAttachmentCommand(id)));

        [HttpGet("{id}/Save")]
        public async Task<ActionResult> SaveAsync(string id)
        {
            var fileModel = await _mediator.Send(new SaveAttachmentQuery(id));
            return File(fileModel.File, fileModel.ContentType, fileModel.Filename.Substring(0, 1));
        }

        [HttpPost("{goalId}")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        public async Task<ActionResult<IAPIResponse<Attachment>>> CreateAsync(string goalId) => Ok(await _mediator.Send(new CreateAttachmentCommand(new AttachmentModel
        {
            GoalId = Guid.Parse(goalId),
            File = Request.Form.Files[0],
        })));
    }
}
