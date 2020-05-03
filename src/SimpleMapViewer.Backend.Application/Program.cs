using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SimpleMapViewer.Backend.Application {
    internal static class Program {
        public static void Main() {
            CreateHostBuilder().Build().Run();
        }

        private static IHostBuilder CreateHostBuilder() {
            var hostBuilder = Host.CreateDefaultBuilder();
            // Configure WebHost
            hostBuilder
                .ConfigureWebHostDefaults(webHostBuilder => {
                    webHostBuilder
                        .UseKestrel()
                        .UseStartup<Startup>();
                });
            // Setup Autofac
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            // Setup Serilog
            hostBuilder.UseSerilog((builderContext, loggerConfig) =>
                loggerConfig.ReadFrom.Configuration(builderContext.Configuration)
            );
            return hostBuilder;
        }
    }
}