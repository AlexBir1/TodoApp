using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Queries.Attachments
{
    public record SaveAttachmentQuery(string attachmentId) : IRequest<FileModel>;
}
