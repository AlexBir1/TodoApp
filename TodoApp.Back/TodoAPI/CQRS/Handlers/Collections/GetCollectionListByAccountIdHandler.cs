using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Queries.Collections;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Collections
{
    public class GetCollectionListByAccountIdHandler : IRequestHandler<GetCollectionListByAccountIdQuery, IAPIResponse<IEnumerable<Collection>>>
    {
        private readonly ICollectionService _collectionService;

        public GetCollectionListByAccountIdHandler(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public async Task<IAPIResponse<IEnumerable<Collection>>> Handle(GetCollectionListByAccountIdQuery request, CancellationToken cancellationToken)
        {
            return await _collectionService.GetAllAsync(x => x.AccountId == request.accountId);
        }
    }
}
