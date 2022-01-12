using AutoMapper;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;

namespace Caffe.Models.Mapers
{
    public class MenuMappingProfile : Profile
    {
        public MenuMappingProfile()
        {
            CreateMap<MenuItemMongoDto, MenuItemDto>();
        }
    }
}