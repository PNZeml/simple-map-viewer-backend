using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp {
    [UnitOfWorkRequired]
    internal class SignUpRequest : IRequest {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}