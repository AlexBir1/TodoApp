using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;

namespace TodoAPI.CQRS.Commands.Collections
{
    public record DeleteCollectionCommand(string collectionId) : IRequest<IAPIResponse<Collection>>;
}
