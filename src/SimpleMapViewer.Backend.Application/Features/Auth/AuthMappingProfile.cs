using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.Dtos;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.Dtos;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn;

namespace SimpleMapViewer.Backend.Application.Features.Auth {
    internal class AuthMappingProfile : Profile {
        public AuthMappingProfile() {
            // Queries
            CreateMap<SignInDto, SignInRequest>();
            // Commands
            CreateMap<SignUpDto, SignUpRequest>();
            CreateMap<SignUpRequest, Domain.Entities.User>();
            // Out dtos
            CreateMap<Domain.Entities.User, UserOutDto>();
        }
    }
}