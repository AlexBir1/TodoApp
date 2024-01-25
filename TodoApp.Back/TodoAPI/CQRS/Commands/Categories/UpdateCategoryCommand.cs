using MediatR;
using TodoAPI.APIResponse.Interfaces;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.CQRS.Commands.Categories
{
    public record UpdateCategoryCommand(string collectionId, CategoryModel model) : IRequest<IAPIResponse<Category>>;
}
