using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryInfrastructure(this IServiceCollection services, string connectionString)
    {
        Action<DbContextOptionsBuilder> configure = o =>
            o.UseLazyLoadingProxies().UseSqlServer(connectionString);
        
        services.AddDbContext<DatabaseContext>(configure);
        services.AddSingleton(new DatabaseContextFactory(configure));
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        
        return services;
    }
}