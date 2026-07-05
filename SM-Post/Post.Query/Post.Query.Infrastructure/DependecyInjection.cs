using Confluent.Kafka;
using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using EventHandler = Post.Query.Infrastructure.Handlers.EventHandler;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryInfrastructure(this IServiceCollection services, string connectionString, ConfigurationManager configuration)
    {
        Action<DbContextOptionsBuilder> configure = o =>
            o.UseLazyLoadingProxies().UseSqlServer(connectionString);
        
        services.AddDbContext<DatabaseContext>(configure);
        services.AddSingleton(new DatabaseContextFactory(configure));
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IEventHandler, EventHandler>();
        services.AddScoped<IEventConsumer, EventConsumer>();
        services.AddHostedService<ConsumerHostedService>();
        
        services.Configure<ConsumerConfig>(configuration.GetSection(nameof(ConsumerConfig)));
        
        return services;
    }
}