using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.User.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User {
    internal class UserMappingProfile : Profile {
        public UserMappingProfile() {
            CreateMap<Domain.Entities.User, UserDto>();
        }
    }
}