using AutoMapper;
using ShareSquare.Data.Models.Domain;
using ShareSquare.Data.Models;

namespace ShareSquare.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReviewDTO, Review>()
             .ForMember(dest => dest.Rating, opts => opts.MapFrom(src => src.Rating))
             .ForMember(dest => dest.Text, opts => opts.MapFrom(src => src.Text))
             .ForMember(dest => dest.Timestamp, opts => opts.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.Created_at, opts => opts.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.Updated_at, opts => opts.Ignore());

            CreateMap<ItemViewModel, Item>()
            .ForMember(dest => dest.ImageUrl, act => act.Ignore()) 
            .ForMember(dest => dest.Status, act => act.Ignore()) 
            .ForMember(dest => dest.User, act => act.Ignore());
        }
    }
}
