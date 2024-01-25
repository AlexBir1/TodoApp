using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Collections;
using TodoAPI.CQRS.Commands.Goals;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Collections
{
    public class CreateCollectionHandler : IRequestHandler<CreateCollectionCommand, IAPIResponse<Collection>>
    {
        private readonly ICollectionService _collectionService;

        public CreateCollectionHandler(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public async Task<IAPIResponse<Collection>> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
        {
            return await _collectionService.CreateAsync(request.model);
        }
    }
}
