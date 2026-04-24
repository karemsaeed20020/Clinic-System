
using Clinic_System.Application.Common;
using Clinic_System.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic_System.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationAssemblyReference).Assembly;

            // AutoMapper & MediatR & FluentValidation
            services.AddAutoMapper(assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationsBehaviors<,>));



            return services;
        }
    }
}
