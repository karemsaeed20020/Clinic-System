
using Clinic_System.Application.Common;
using Clinic_System.Application.Common.Behaviours;
using Clinic_System.Application.Service.Implemention;
using Clinic_System.Application.Service.Interface;
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
            // Core Business Services
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientService, PatientService>();


            return services;
        }
    }
}
