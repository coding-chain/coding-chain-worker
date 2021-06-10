using System;
using System.Reflection;
using Application.Contracts.Factories;
using Application.Contracts.IService;
using Application.Contracts.Processes;
using Application.ParticipationExecution;
using Application.PlagiarismAnalyze;
using CodingChainApi.Infrastructure.Factories;
using CodingChainApi.Infrastructure.Logs;
using CodingChainApi.Infrastructure.Messaging;
using CodingChainApi.Infrastructure.Services;
using CodingChainApi.Infrastructure.Services.Processes;
using CodingChainApi.Infrastructure.Services.RightElevator;
using CodingChainApi.Infrastructure.Services.TestsParsers;
using CodingChainApi.Infrastructure.Settings;
using Domain.Plagiarism;
using Domain.TestExecution.Imperative.Typescript;
using Domain.TestExecution.OOP.CSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace CodingChainApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IProcessServiceFactory, ProcessServiceFactory>();
            services.AddScoped<IParticipationAggregateFactory, ParticipationAggregateFactory>();
            services.AddScoped<IDirectoryService, DirectoryService>();
            services.ConfigureRightElevator(configuration);
            services.ConfigureTestsParsers(configuration);
            services.ConfigureProcessServices(configuration);
            ConfigureInjectableSettings<IAssetsSettings, AssetsSettings>(services, configuration);
            ConfigureInjectableSettings<IPlagiarismSettings, PlagiarismSettings>(services, configuration);
            ConfigureInjectableSettings<ICSharpExecutionSettings, CSharpExecutionSettings>(services, configuration);
            ConfigureInjectableSettings<ITypescriptExecutionSettings, TypescriptExecutionSettings>(services,
                configuration);
            ConfigureInjectableSettings<ITemplateSettings, TemplateSettings>(services, configuration);
            ConfigureRabbitMqSettings(services, configuration);
            ConfigureElasticSearch(services, configuration);
            return services;
        }

        private static void ConfigureProcessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<WindowsTypescriptProcessService>();
            services.AddScoped<UnixTypescriptProcessService>();
            services.AddScoped<ITypescriptProcessService>(provider =>
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return provider.GetRequiredService<WindowsTypescriptProcessService>();
                }
                return provider.GetRequiredService<UnixTypescriptProcessService>();
            });
            services.AddScoped<ICsharpProcessService, CsharpProcessService>();
        }

        private static void ConfigureTestsParsers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<WindowsJestTestsParser>();
            services.AddScoped<UnixJestTestsParser>();
            services.AddScoped<ICsharpTestsParser, NUnitTestsParser>();
            services.AddScoped<ITypescriptTestsParser>(provider =>
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return provider.GetRequiredService<WindowsJestTestsParser>();
                }
                return provider.GetRequiredService<UnixJestTestsParser>();
            });
        }
        
        private static void ConfigureRightElevator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<WindowsRightElevatorService>();
            services.AddScoped<MacRightElevatorService>();
            services.AddScoped<UnixRightElevatorService>();
            services.AddScoped<IRightElevatorService>(provider =>
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    return provider.GetRequiredService<WindowsRightElevatorService>();
                }

                if (Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    return provider.GetRequiredService<MacRightElevatorService>();
                }

                return provider.GetRequiredService<UnixRightElevatorService>();
            });
        }

        private static TImplementation ConfigureInjectableSettings<TInterface, TImplementation>(
            IServiceCollection services,
            IConfiguration configuration, bool singleton = true) where TImplementation : class, TInterface
            where TInterface : class
        {
            var settingsName = typeof(TImplementation).Name;
            var settings = configuration.GetSection(settingsName).Get<TImplementation>();
            services.Configure<TImplementation>(configuration.GetSection(settingsName));
            if (singleton)
                services.AddSingleton<TInterface>(sp =>
                    sp.GetRequiredService<IOptions<TImplementation>>().Value);
            else
                services.AddScoped<TInterface>(sp =>
                    sp.GetRequiredService<IOptions<TImplementation>>().Value);

            return settings;
        }

        private static void ConfigureElasticSearch(IServiceCollection services, IConfiguration configuration)
        {
            var elasticSearchSettings =
                ConfigureInjectableSettings<IElasticSearchSettings, ElasticSearchSettings>(services, configuration);
            var settings = new ConnectionSettings(new Uri(elasticSearchSettings.Url))
                .DefaultIndex(elasticSearchSettings.CodeProcessResponseLogIndex);

            var client = new ElasticClient(settings);

            client.Indices.Create(elasticSearchSettings.CodeProcessResponseLogIndex,
                index =>
                {
                    index.Map<CodeProcessResponseLog>(x => x.AutoMap());
                    index.Settings(descriptor => descriptor.BlocksReadOnlyAllowDelete(false));
                    return index;
                }
            );
            services.AddSingleton<IElasticClient>(client);
        }

        private static void ConfigureRabbitMqSettings(IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IDispatcher<CodeProcessResponse>, ParticipationDoneResponseService>();
            services.AddScoped<IDispatcher<PlagiarismAnalyzeResponse>, PlagiarismDoneResponseService>();
            services.AddScoped<IDispatcher<PreparedParticipationResponse>, PreparedParticipationResponseService>();
            ConfigureInjectableSettings<IRabbitMqSettings, RabbitMqSettings>(services, configuration);
        }
    }
}