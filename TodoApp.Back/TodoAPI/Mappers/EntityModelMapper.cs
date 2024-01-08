using AutoMapper;
using TodoAPI.DAL.Entities;
using TodoAPI.Shared.Models;

namespace TodoAPI.Mappers
{
    public class EntityModelMapper : Profile
    {
        public EntityModelMapper()
        {
            CreateMap<GoalModel, Goal>()
                .ForMember(x => x.Id, x => x.MapFrom(_ => Guid.NewGuid()))
                .ForMember(x => x.Title, x => x.MapFrom(c => c.Title))
                .ForMember(x => x.CollectionId, x => x.MapFrom(c => c.CollectionId))
                .ForMember(x => x.StartDate, x => x.MapFrom(c => c.StartDate))
                .ForMember(x => x.CreationDate, x => x.MapFrom(c => c.CreationDate))
                .ForMember(x => x.UpdateDate, x => x.MapFrom(c => c.UpdateDate));

            CreateMap<CollectionModel, Collection>()
                .ForMember(x => x.Title, x => x.MapFrom(c => c.Title))
                .ForMember(x => x.AccountId, x => x.MapFrom(c => c.AccountId));

            CreateMap<CategoryModel, Category>()
                .ForMember(x => x.ColorTitle, x => x.MapFrom(c => c.ColorTitle))
                .ForMember(x => x.ColorHex, x => x.MapFrom(c => c.ColorHex));
        }
    }
}
