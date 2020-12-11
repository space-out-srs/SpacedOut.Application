using Microsoft.Extensions.DependencyInjection;
using SpacedOut.Domain.Cards;
using SpacedOut.Infrastucture.Data;
using SpacedOut.Infrastucture.DomainEvents;
using SpacedOut.Infrastucture.Processing.Outbox;
using SpacedOut.SharedKernal.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpacedOut.Infrastucture.Infrastructure
{
    public static class DefaultInfrastructureModuleHelpers
    {
        public static void RegisterDefaultInfrastructureModule(this IServiceCollection services, bool isDevelopment, Assembly? callingAssembly = null)
        {
            _ = new DefaultInfrastructureModule(
                services,
                isDevelopment,
                callingAssembly
            );
        }
    }

    public class DefaultInfrastructureModule
    {
        private readonly bool _isDevelopment = false;
        private readonly List<Assembly> _assemblies = new List<Assembly>();

        public DefaultInfrastructureModule(IServiceCollection services, bool isDevelopment, Assembly? callingAssembly = null)
        {
            _isDevelopment = isDevelopment;

            Initialize(callingAssembly);

            Load(services);
        }

        private void Initialize(Assembly? callingAssembly = null)
        {
            var domainAssembly = Assembly.GetAssembly(typeof(Card));
            if (domainAssembly != null)
            {
                _assemblies.Add(domainAssembly);
            }

            var infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository));
            if (infrastructureAssembly != null)
            {
                _assemblies.Add(infrastructureAssembly);
            }

            if (callingAssembly != null)
            {
                _assemblies.Add(callingAssembly);
            }
        }

        private void Load(IServiceCollection services)
        {
            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(services);
            }
            else
            {
                RegisterProductionOnlyDependencies(services);
            }

            RegisterCommonDependencies(services);
        }

        private void RegisterCommonDependencies(IServiceCollection services)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IRepository, EfRepository>();
            services.AddScoped<IUnitOfWorkFactory, EfTransactionFactory>();

            RegisterAssembliesAsClosedTypesOf(services, typeof(IHandle<>));

            RegisterOutbox(services);
        }

        /*
         * https://stackoverflow.com/a/63227626/234132
         * https://stackoverflow.com/a/42729391/234132
         */
        private void RegisterAssembliesAsClosedTypesOf(IServiceCollection services, Type genericType)
        {
            foreach (var assembly in _assemblies)
            {
                services.Scan(scan => scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes.AssignableTo(genericType))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                );
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "'services' will be needed to register future dependencies")]
        private static void RegisterDevelopmentOnlyDependencies(IServiceCollection services)
        {
            // TODO: Add development only services
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "'services' will be needed to register future dependencies")]
        private static void RegisterProductionOnlyDependencies(IServiceCollection services)
        {
            // TODO: Add production only services
        }

        private void RegisterOutbox(IServiceCollection services)
        {
            RegisterAssembliesAsClosedTypesOf(services, typeof(BaseOutboxMessageHandler));
            services.AddHostedService<ProcessOutboxQueueJob>();
        }
    }
}