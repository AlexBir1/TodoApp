using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Categories
{
    public record CreateCategoryCommand(CategoryModel model) : IRequest<IAPIResponse<Category>>;
}
