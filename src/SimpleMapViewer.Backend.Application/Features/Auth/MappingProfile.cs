using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Backend.Application.Features.Auth {
    internal class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<User, SignInResponse>();
            CreateMap<SignUpRequest, User>();
        }
    }
}