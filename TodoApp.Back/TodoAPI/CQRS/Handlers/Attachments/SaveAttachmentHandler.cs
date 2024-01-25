using MediatR;
using TodoAPI.CQRS.Queries.Attachments;
using TodoAPI.Services.Interfaces;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Handlers.Attachments
{
    public class SaveAttachmentHandler : IRequestHandler<SaveAttachmentQuery, FileModel>
    {
        private readonly IAttachmentService _attachmentService;

        public SaveAttachmentHandler(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        public async Task<FileModel> Handle(SaveAttachmentQuery request, CancellationToken cancellationToken)
        {
            return await _attachmentService.DownloadAsync(request.attachmentId);
        }
    }
}
