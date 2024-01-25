using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.CQRS.Commands.Categories;
using TodoAPI.DAL.Entities;
using TodoAPI.Services.Interfaces;

namespace TodoAPI.CQRS.Handlers.Categories
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, IAPIResponse<Category>>
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IAPIResponse<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryService.CreateAsync(request.model);
        }
    }
}
