using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using AutoMapper.Configuration;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using Module = Autofac.Module;

namespace SimpleMapViewer.Backend.Application.IoC {
    public class AutoMapperModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .Register(CreateMapper)
                .As<IMapper>()
                .SingleInstance();
        }

        private IMapper CreateMapper(IComponentContext context) {
            var resolvedContext = context.Resolve<IComponentContext>();
            var mapperConfiguration = new MapperConfiguration(
                configuration => {
                    var assemblies = new[] { Assembly.GetExecutingAssembly() };
                    var profileConfigurations = assemblies.SelectMany(x =>
                        x.GetTypesOfInterface(typeof(IProfileConfiguration))
                    );

                    foreach (var profileConfiguration in profileConfigurations) {
                        configuration.AddProfile(profileConfiguration);
                    }

                    configuration.ConstructServicesUsing(resolvedContext.Resolve);
                }
            );

            return mapperConfiguration.CreateMapper();
        }
    }
}