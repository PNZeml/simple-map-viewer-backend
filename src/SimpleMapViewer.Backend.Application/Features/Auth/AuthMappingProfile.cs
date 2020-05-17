using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp;
using SimpleMapViewer.Backend.Application.Features.Auth.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth {
    internal class AuthMappingProfile : Profile {
        public AuthMappingProfile() {
            CreateMap<Domain.Entities.User, UserDto>();
            CreateMap<SignUpRequest, Domain.Entities.User>();
        }
    }
}