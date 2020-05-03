using System;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace SimpleMapViewer.Backend.Application.IoC {
    internal class AppModule : Module {
        private readonly IConfiguration configuration;

        public AppModule(IConfiguration configuration) {
            this.configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder) {
            // Common modules
            builder.RegisterModule<AutoMapperModule>();
            builder.RegisterModule<DatabaseModule>();
            builder.RegisterModule<MediatrModule>();
            builder.RegisterModule(new SettingsModule(configuration));
            // Lifetime factory
            builder
                .Register<Func<object, ILifetimeScope>>(
                    context => {
                        var lifetimeScope = context.Resolve<ILifetimeScope>();
                        return tag => lifetimeScope.BeginLifetimeScope(tag);
                    }
                )
                .InstancePerLifetimeScope();
        }
    }
}