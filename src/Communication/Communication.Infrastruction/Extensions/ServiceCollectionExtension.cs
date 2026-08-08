using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Infrastruction.Data;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Infrastruction.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CommunicationContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IGenericRepository<Community, int>, GenericRepository<Community, int>>();
        services.AddScoped<IGenericRepository<CommunityDiscussion, int>, GenericRepository<CommunityDiscussion, int>>();

        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<IInviteToCommunityRepository, InviteToCommunityRepository>();
        services.AddScoped<ICommunityDiscussionRepository, CommunityDiscussionRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
