using Autofac;
using Microsoft.Extensions.Configuration;
using SimpleMapViewer.Backend.Application.Settings;
using SimpleMapViewer.Infrastructure.Settings;

namespace SimpleMapViewer.Backend.Application.IoC {
    internal class SettingsModule : Module {
        private readonly IConfiguration configuration;

        public SettingsModule(IConfiguration configuration) {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder) {
            var connectionString = configuration.GetSection("Database")["ConnectionString"];
            var databaseSettings = new DatabaseSettings(connectionString);
            builder
                .RegisterInstance(databaseSettings)
                .As<IDatabaseSettings>()
                .SingleInstance();
        }
    }
}