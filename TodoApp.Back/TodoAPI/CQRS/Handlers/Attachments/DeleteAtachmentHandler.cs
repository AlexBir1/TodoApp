using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Attachments;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Attachments
{
    public class DeleteAtachmentHandler : IRequestHandler<DeleteAttachmentCommand, IAPIResponse<Attachment>>
    {
        private readonly IAttachmentService _attachmentService;

        public DeleteAtachmentHandler(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        public async Task<IAPIResponse<Attachment>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            return await _attachmentService.DeleteAsync(request.attachmentId);
        }
    }
}
