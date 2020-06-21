using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn {
    [UnitOfWorkRequired]
    internal class SignInRequest : IRequest<SignInResponse> {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}