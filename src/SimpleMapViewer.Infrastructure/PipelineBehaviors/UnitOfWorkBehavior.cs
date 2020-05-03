using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using NHibernate;
using Shura.Data;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Infrastructure.PipelineBehaviors {
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private static readonly UnitOfWorkRequiredAttribute UnitOfWorkRequiredAttribute =
            typeof(TRequest).GetCustomAttribute<UnitOfWorkRequiredAttribute>();

        private readonly ILifetimeScope lifetimeScope;

        public UnitOfWorkBehavior(ILifetimeScope lifetimeScope) {
            this.lifetimeScope = lifetimeScope;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        ) {
            if (UnitOfWorkRequiredAttribute == null) return await next();

            var uow = lifetimeScope.Resolve<IUnitOfWork<ISession>>();

            if (uow.State == UnitOfWorkState.Ready) {
                uow.Begin(UnitOfWorkRequiredAttribute.IsolationLevel);
            }

            var response = await next();

            if (uow.State == UnitOfWorkState.Begun) {
                await uow.CommitAsync();
            }

            return response;
        }
    }
}