using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Collections;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Collections
{
    public class UpdateCollectionHandler : IRequestHandler<UpdateCollectionCommand, IAPIResponse<Collection>>
    {
        private readonly ICollectionService _collectionService;

        public UpdateCollectionHandler(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public async Task<IAPIResponse<Collection>> Handle(UpdateCollectionCommand request, CancellationToken cancellationToken)
        {
            return await _collectionService.UpdateAsync(request.collectionId, request.model);
        }
    }
}
