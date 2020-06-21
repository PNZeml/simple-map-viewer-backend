using FluentValidation;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Validators {
    public class SignInValidator : AbstractValidator<SignInDto> {
        public SignInValidator() {
            RuleFor(x => x.Login)
                .Length(4, 250);
            RuleFor(x => x.Password)
                .Length(5, 250);
        }
    }
}