using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Queries.Attachments
{
    public record GetAttachmentListByGoalIdQuery(string goalId) : IRequest<IAPIResponse<IEnumerable<Attachment>>>;
}
