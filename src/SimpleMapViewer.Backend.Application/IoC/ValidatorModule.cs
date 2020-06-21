using System.Reflection;
using Autofac;
using SimpleMapViewer.Backend.Application.Common;
using Module = Autofac.Module;

namespace SimpleMapViewer.Backend.Application.IoC {
    internal class ValidatorModule : Module {
        protected override void Load(ContainerBuilder builder) {
            var mediatorBaseValidatorType = typeof(RequestBaseValidator<>);
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(mediatorBaseValidatorType)
                .AsImplementedInterfaces();
        }
    }
}