using ABC.Management.Domain.Entities;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedKernell;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ABC.PostGreSQL.Extensions;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddABCPostGreSQL(
        this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddDbContextFactory<ABCContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("abcdata")
                ?? throw new InvalidOperationException("Connection string 'abcdata' not found.")));

        services.AddScoped<UnitOfWork>();
        services.AddScoped<IEntityService<Antecedent>, AntecedentService>();
        services.AddScoped<IEntityService<Behavior>, BehaviorService>();
        services.AddScoped<IEntityService<Consequence>, ConsequenceService>();

        return builder;
    }
}
