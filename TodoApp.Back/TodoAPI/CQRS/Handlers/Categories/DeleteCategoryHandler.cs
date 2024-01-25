using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Categories;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Categories
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, IAPIResponse<Category>>
    {
        private readonly ICategoryService _categoryService;

        public DeleteCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IAPIResponse<Category>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryService.DeleteAsync(request.collectionId);
        }
    }
}
