using System.Reflection;
using Application.Contracts.IService;
using CodingChainApi.Infrastructure.MessageBroker;
using CodingChainApi.Infrastructure.Messaging;
using CodingChainApi.Infrastructure.Services;
using CodingChainApi.Infrastructure.Services.Processes;
using CodingChainApi.Infrastructure.Settings;
using Domain.Plagiarism;
using Domain.TestExecution.OOP.CSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodingChainApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IProcessService<CSharpParticipationTestingAggregate>, CsharpProcessService>();
            services.AddScoped<IParticipationDoneService, ParticipationDoneResponseService>();
            services.AddScoped<IDirectoryService, DirectoryService>();
            ConfigureInjectableSettings<IAppDataSettings, AppDataSettings>(services, configuration);
            ConfigureInjectableSettings<IPlagiarismSettings, PlagiarismSettings>(services, configuration);
            ConfigureInjectableSettings<ICSharpExecutionSettings, CSharpExecutionSettings>(services, configuration);
            ConfigureInjectableSettings<ITemplateSettings, TemplateSettings>(services, configuration);
            ConfigureRabbitMqSettings(services, configuration);
            return services;
        }

        private static TImplementation ConfigureInjectableSettings<TInterface, TImplementation>(
            IServiceCollection services,
            IConfiguration configuration, bool singleton = false) where TImplementation : class, TInterface
            where TInterface : class
        {
            var settingsName = typeof(TImplementation).Name;
            var settings = configuration.GetSection(settingsName).Get<TImplementation>();
            services.Configure<TImplementation>(configuration.GetSection(settingsName));
            if (singleton)
            {
                services.AddSingleton<TInterface>(sp =>
                    sp.GetRequiredService<IOptions<TImplementation>>().Value);
            }
            else
            {
                services.AddScoped<TInterface>(sp =>
                    sp.GetRequiredService<IOptions<TImplementation>>().Value);
            }

            return settings;
        }

        private static void ConfigureRabbitMqSettings(IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            // RabbitMQ
            ConfigureInjectableSettings<IRabbitMqSettings, RabbitMqSettings>(serviceCollection, configuration, true);
            // End RabbitMQ Configuration
        }
    }
}