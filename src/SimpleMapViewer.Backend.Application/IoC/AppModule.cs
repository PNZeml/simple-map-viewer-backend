using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using SimpleMapViewer.Backend.Application.Common.AvatarGenerator;
using SimpleMapViewer.Backend.Application.Features.Map.IoC;

namespace SimpleMapViewer.Backend.Application.IoC {
    internal class AppModule : Module {
        private readonly IConfiguration _configuration;

        public AppModule(IConfiguration configuration) {
            _configuration = configuration;
        }
        
        protected override void Load(ContainerBuilder builder) {
            // Common modules
            builder.RegisterModule<AutoMapperModule>();
            builder.RegisterModule<DatabaseModule>();
            builder.RegisterModule<MediatorModule>();
            builder.RegisterModule<ValidatorModule>();
            builder.RegisterModule(new SettingsModule(_configuration));
            // Common
            builder
                .RegisterInstance(AvatarGeneratorBuilder.Build(500))
                .SingleInstance();
            // Features
            builder.RegisterModule<MapModule>();
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