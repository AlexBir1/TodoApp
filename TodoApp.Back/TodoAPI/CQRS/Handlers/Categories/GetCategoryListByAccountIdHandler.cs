using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Queries.Categories;
using TodoAPI.CQRS.Queries.Collections;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Categories
{
    public class GetCategoryListByAccountIdHandler : IRequestHandler<GetCategoryListByAccountIdQuery, IAPIResponse<IEnumerable<Category>>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryListByAccountIdHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IAPIResponse<IEnumerable<Category>>> Handle(GetCategoryListByAccountIdQuery request, CancellationToken cancellationToken)
        {
            return await _categoryService.GetAllAsync(x => x.AccountId == request.accountId);
        }
    }
}
