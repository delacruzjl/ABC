using System.Diagnostics.CodeAnalysis;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ABC.PostGreSQL.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IHostApplicationBuilder AddABCPostGreSQL(
        this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        var connectionName = builder.Configuration["databaseName"];
        services.AddDbContextFactory<ABCContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(connectionName!)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEntityService<Antecedent>, AntecedentService>();
        services.AddScoped<IEntityService<Behavior>, BehaviorService>();
        services.AddScoped<IEntityService<Consequence>, ConsequenceService>();
        services.AddScoped<IEntityService<ChildCondition>, ChildConditionService>();

        return builder;
    }
}
