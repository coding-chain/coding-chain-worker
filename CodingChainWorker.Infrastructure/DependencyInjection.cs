﻿using System;
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
using CodingChainApi.Infrastructure.Settings;
using Domain.Plagiarism;
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
            services.AddScoped<TypescriptProcessService>();
            services.AddScoped<CsharpProcessService>();
            services.AddScoped<IRightElevatorService, UnixElevatorService>();

            services.AddScoped<IDirectoryService, DirectoryService>();
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

        private static void ConfigureRightElevator(IServiceCollection services, IConfiguration configuration)
        {
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
                return provider.GetRequiredService<UnixElevatorService>();
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