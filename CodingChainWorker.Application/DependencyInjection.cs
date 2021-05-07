using System.Reflection;
using Application.Common.Behaviors;
using Application.Common.Events;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
            services.AddMediatR(typeof(MediatrDomainEventDispatcher).GetTypeInfo().Assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}