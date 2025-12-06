using Microsoft.Extensions.DependencyInjection;
using VeterinaryClinic.Application.Interfaces;
using VeterinaryClinic.Application.Services;
using VeterinaryClinic.Application.Mappings;

namespace VeterinaryClinic.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IAppointmentService, AppointmentService>();

            return services;
        }
    }
}