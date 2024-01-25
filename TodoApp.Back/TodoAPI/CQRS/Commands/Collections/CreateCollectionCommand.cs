using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Collections
{
    public record CreateCollectionCommand(CollectionModel model) : IRequest<IAPIResponse<Collection>>;
}
