using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Map {
    internal class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Domain.Entities.User, UserInDto>();
            CreateMap<Domain.Entities.GeoFileComment, GeoFileCommentOutDto>();
            CreateMap<UserInDto, UserOutDto>();
        }
    }
}