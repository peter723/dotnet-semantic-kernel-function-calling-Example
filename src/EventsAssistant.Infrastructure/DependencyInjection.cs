using EventsAssistant.Application.Abstractions;
using EventsAssistant.Application.Services;
using EventsAssistant.Infrastructure.Persistence;
using EventsAssistant.Infrastructure.SemanticKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsAssistant.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=events-assistant.db";

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IEventRepository, EfEventRepository>();
        services.AddScoped<EventService>();
        services.AddScoped<EventsPlugin>();
        services.AddScoped<IChatAssistant, SemanticKernelChatAssistant>();

        return services;
    }
}
