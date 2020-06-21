using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsers;

namespace SimpleMapViewer.Backend.Application.Features.User {
    internal class UserMappingProfile : Profile {
        public UserMappingProfile() {
            CreateMap<Domain.Entities.User, UserOutDto>();
            // Queries
            CreateMap<GetAllUsersInDto, GetAllUsersRequest>();
        }
    }
}