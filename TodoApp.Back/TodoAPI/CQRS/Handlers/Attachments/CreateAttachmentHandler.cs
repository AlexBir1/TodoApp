using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Attachments;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Attachments
{
    public class CreateAttachmentHandler : IRequestHandler<CreateAttachmentCommand, IAPIResponse<Attachment>>
    {
        private readonly IAttachmentService _attachmentService;

        public CreateAttachmentHandler(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        public async Task<IAPIResponse<Attachment>> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
        {
            return await _attachmentService.CreateAsync(request.model);
        }
    }
}
