using Autofac;
using FluentValidation;

namespace SimpleMapViewer.Backend.Application.Common {
    internal abstract class RequestBaseValidator<TRequest> : AbstractValidator<TRequest> {
        internal readonly ILifetimeScope LifetimeScope;

        public RequestBaseValidator(ILifetimeScope lifetimeScope) {
            LifetimeScope = lifetimeScope;
        }
    }
}