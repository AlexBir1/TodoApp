using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Queries.Attachments;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Attachments
{
    public class GetAttachmentListByGoalIdHandler : IRequestHandler<GetAttachmentListByGoalIdQuery, IAPIResponse<IEnumerable<Attachment>>>
    {
        private readonly IAttachmentService _attachmentService;

        public GetAttachmentListByGoalIdHandler(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        public async Task<IAPIResponse<IEnumerable<Attachment>>> Handle(GetAttachmentListByGoalIdQuery request, CancellationToken cancellationToken)
        {
            return await _attachmentService.GetAllByGoalIdAsync(request.goalId);
        }
    }
}
