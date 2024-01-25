using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Collections;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Collections
{
    public class DeleteCollectionHandler : IRequestHandler<DeleteCollectionCommand, IAPIResponse<Collection>>
    {
        private readonly ICollectionService _collectionService;

        public DeleteCollectionHandler(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public async Task<IAPIResponse<Collection>> Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
        {
            return await _collectionService.DeleteAsync(request.collectionId);
        }
    }
}
