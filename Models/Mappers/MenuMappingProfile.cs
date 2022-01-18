using AutoMapper;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;

namespace Caffe.Models.Mappers
{
    public class MenuMappingProfile : Profile
    {
        public MenuMappingProfile()
        {
            CreateMap<MenuItemMongoDto, MenuItemDto>();
        }
    }
}