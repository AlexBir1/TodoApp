using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Attachments
{
    public record CreateAttachmentCommand(AttachmentModel model) : IRequest<IAPIResponse<Attachment>>;
}
