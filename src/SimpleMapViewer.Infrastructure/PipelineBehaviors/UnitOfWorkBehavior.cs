using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using NHibernate;
using Serilog;
using Shura.Data;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Infrastructure.PipelineBehaviors {
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private static readonly UnitOfWorkRequiredAttribute UnitOfWorkRequiredAttribute =
            typeof(TRequest).GetCustomAttribute<UnitOfWorkRequiredAttribute>();

        private readonly ILifetimeScope _lifetimeScope;

        public UnitOfWorkBehavior(ILifetimeScope lifetimeScope) {
            _lifetimeScope = lifetimeScope;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        ) {
            if (UnitOfWorkRequiredAttribute == null) return await next();

            var unitOfWork = _lifetimeScope.Resolve<IUnitOfWork<ISession>>();

            if (unitOfWork.State == UnitOfWorkState.Ready) {
                unitOfWork.Begin(UnitOfWorkRequiredAttribute.IsolationLevel);
            }

            try {
                var response = await next();

                if (unitOfWork.State == UnitOfWorkState.Begun) {
                    await unitOfWork.CommitAsync();
                }

                return response;
            } catch (Exception exception) {
                await unitOfWork.RollbackAsync();
                Log.Error(exception, "An error occured during unit of work execution");
                throw;
            }
        }
    }
}