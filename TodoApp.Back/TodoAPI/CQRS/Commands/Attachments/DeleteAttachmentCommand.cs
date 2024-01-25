using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Commands.Attachments
{
    public record DeleteAttachmentCommand(string attachmentId) : IRequest<IAPIResponse<Attachment>>;
}
