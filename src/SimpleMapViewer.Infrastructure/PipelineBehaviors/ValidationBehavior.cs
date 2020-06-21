using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Serilog;
using SimpleMapViewer.Infrastructure.Attributes;
using SimpleMapViewer.Infrastructure.Exceptions;

namespace SimpleMapViewer.Infrastructure.PipelineBehaviors {
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly IValidator<TRequest> _validator;

        private static readonly bool IsValidationRequired =
            typeof(TRequest).IsDefined(typeof(ValidationRequiredAttribute), false);

        public ValidationBehavior(IValidator<TRequest> validator = default) {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        ) {
            if (IsValidationRequired) {
                await ValidateRequest(request, cancellationToken);
            }

            return await next();
        }

        private async Task ValidateRequest(TRequest request, CancellationToken cancellationToken) {
            try {
                await _validator.ValidateAndThrowAsync(
                    request,
                    cancellationToken: cancellationToken
                );
            } catch (ValidationException exception) {
                Log.Error(exception, "A request has an invalid payload.");
                throw;
            }
        }
    }
}