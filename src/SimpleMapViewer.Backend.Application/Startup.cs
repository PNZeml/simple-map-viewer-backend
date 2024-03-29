using System.Reflection;
using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using SimpleMapViewer.Backend.Application.Common.Constants;
using SimpleMapViewer.Backend.Application.Features.Auth.AuthenticationHandlers;
using SimpleMapViewer.Backend.Application.Features.Map.Hub;
using SimpleMapViewer.Backend.Application.IoC;
using SimpleMapViewer.Backend.Application.Settings;

namespace SimpleMapViewer.Backend.Application {
    public class Startup {
        private readonly IConfiguration _configuration;

        public Startup(IHostEnvironment hostingEnvironment) {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json")
                .AddEnvironmentVariables();
            _configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            ConfigureAuth(services);
            ConfigureCors(services);
            ConfigureRoutingAndMvc(services);
            ConfigureSwagger(services);
        } 

        public void ConfigureContainer(ContainerBuilder containerBuilder) {
            containerBuilder.RegisterModule(new AppModule(_configuration));
        }

        public void Configure(
            IApplicationBuilder applicationBuilder,
            IHostEnvironment hostEnvironment
        ) {
            if (hostEnvironment.IsDevelopment()) {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseCors("AllowAll");
            applicationBuilder.UseWebSockets();
            applicationBuilder.UseRouting();
            applicationBuilder
                .UseAuthentication()
                .UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<MapHub>("/hubs/v1/map");
            });
            applicationBuilder
                .UseOpenApi()
                .UseSwaggerUi3();
        }

        private void ConfigureAuth(IServiceCollection services) {
            // Authentication
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = AuthenticationSchemes.TOKEN_AUTH_SCHEMA;
                    options.DefaultChallengeScheme = AuthenticationSchemes.TOKEN_AUTH_SCHEMA;
                })
                .AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>(
                    AuthenticationSchemes.TOKEN_AUTH_SCHEMA,
                    null
                );
            // Authorization
            var authorizationPolicy =
                new AuthorizationPolicyBuilder(AuthenticationSchemes.TOKEN_AUTH_SCHEMA)
                    .RequireAuthenticatedUser()
                    .Build();
            services.AddAuthorization(options => {
                options.AddPolicy(AuthenticationSchemes.TOKEN_AUTH_SCHEMA, authorizationPolicy);
            });
            // Misc
            services.AddHttpContextAccessor();
        }

        private void ConfigureCors(IServiceCollection services) {
            var appSettingsSection = _configuration.GetSection("App");
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddCors(options => {
                options.AddPolicy("AllowAll", policyBuilder => {
                    policyBuilder
                        .WithOrigins(appSettings.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        private void ConfigureRoutingAndMvc(IServiceCollection services) {
            // SignalR
            services.AddSignalR(x => {
                x.EnableDetailedErrors = true;
            });
            // Routing
            services.AddRouting(options => options.LowercaseUrls = true);
            // MVC
            services
                .AddMvc(options => {
                    var authorizeFilter =
                        new AuthorizeFilter(AuthenticationSchemes.TOKEN_AUTH_SCHEMA);
                    options.Filters.Add(authorizeFilter);
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddNewtonsoftJson()
                .AddControllersAsServices()
                .AddFluentValidation(x => {
                    x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    x.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        private void ConfigureSwagger(IServiceCollection services) {
            services.AddSwaggerDocument(swaggerConfig => {
                swaggerConfig.Version = "v1";
                swaggerConfig.DocumentName = "docs";
                swaggerConfig.OperationProcessors.Add(new OperationSecurityScopeProcessor("Token"));
                swaggerConfig.DocumentProcessors.Add(
                    new SecurityDefinitionAppender(
                        "Token",
                        new OpenApiSecurityScheme {
                            Type = OpenApiSecuritySchemeType.ApiKey,
                            Name = "Authorization",
                            Description = "Token authentication. Example: \"Token **TOKEN**\"",
                            In = OpenApiSecurityApiKeyLocation.Header
                        }
                    )
                );
            });
        }
    }
}