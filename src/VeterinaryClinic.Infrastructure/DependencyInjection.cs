using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VeterinaryClinic.Domain.Ports.Out;
using VeterinaryClinic.Infrastructure.Persistence.Context;
using VeterinaryClinic.Infrastructure.Persistence.Repositories;

namespace VeterinaryClinic.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var host = Environment.GetEnvironmentVariable("BD_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT") ;
        var database = Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var connectionString = $"Server={host};Port={port};Database={database};User={user};Password={password};";        
        Console.WriteLine($"Connection String: {connectionString}");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 0))
            )
        );

        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}