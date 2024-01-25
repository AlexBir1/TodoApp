using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Collections
{
    public record UpdateCollectionCommand(string collectionId, CollectionModel model) : IRequest<IAPIResponse<Collection>>;
}
