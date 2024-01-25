using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Categories;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Categories
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, IAPIResponse<Category>>
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IAPIResponse<Category>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryService.UpdateAsync(request.collectionId, request.model);
        }
    }
}
