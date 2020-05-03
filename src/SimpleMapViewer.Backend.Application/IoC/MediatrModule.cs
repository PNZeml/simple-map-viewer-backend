using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using SimpleMapViewer.Infrastructure.PipelineBehaviors;
using Module = Autofac.Module;

namespace SimpleMapViewer.Backend.Application.IoC {
    internal class MediatrModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            builder
                .Register<ServiceFactory>(context => {
                    var resolvedContext = context.Resolve<IComponentContext>();
                    return type => resolvedContext.Resolve(type);
                });

            var mediatrOpenTypes = new[] {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>)
            };
            foreach (var mediatrOpenType in mediatrOpenTypes) {
                builder
                    .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder
                .RegisterGeneric(typeof(UnitOfWorkBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));
        }
    }
}