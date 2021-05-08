using System.Reflection;
using Application.Contracts.IService;
using CodingChainApi.Infrastructure.Services;
using CodingChainApi.Infrastructure.Services.Processes;
using CodingChainApi.Infrastructure.Settings;
using Domain.TestExecution;
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
            ConfigureInjectableSettings<IAppDataSettings, AppDataSettings>(services, configuration);
            ConfigureInjectableSettings<ICSharpExecutionSettings, CSharpExecutionSettings>(services, configuration);
            return services;
        }
        private static TImplementation ConfigureInjectableSettings<TInterface, TImplementation>(
            IServiceCollection services,
            IConfiguration configuration) where TImplementation : class, TInterface where TInterface : class
        {
            var settingsName = typeof(TImplementation).Name;
            var settings = configuration.GetSection(settingsName).Get<TImplementation>();
            services.Configure<TImplementation>(configuration.GetSection(settingsName));
            services.AddSingleton<TInterface>(sp =>
                sp.GetRequiredService<IOptions<TImplementation>>().Value);
            return settings;
        }
    }
}