using Autofac;
using SimpleMapViewer.Backend.Application.Features.Map.Controller;

namespace SimpleMapViewer.Backend.Application.Features.Map.IoC {
    internal class MapModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterType<MapHubController>()
                .As<IMapHubController>()
                .SingleInstance();
        }
    }
}