using FluentValidation;
using FluentValidation.Validators;
using SimpleMapViewer.Backend.Application.Features.Auth.Commands.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Validators {
    public class SignUpValidator : AbstractValidator<SignUpDto> {
        public SignUpValidator() {
            RuleFor(x => x.Name)
                .Length(5, 250);
            RuleFor(x => x.Email)
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible);
            RuleFor(x => x.Password)
                .Length(5, 250);
        }
    }
}