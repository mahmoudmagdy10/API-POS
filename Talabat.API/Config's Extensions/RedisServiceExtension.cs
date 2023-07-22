using StackExchange.Redis;
using Talabat.Core.IRepository;
using Talabat.Repository.Repositories;

namespace Talabat.API.Config_s_Extensions
{
    public static class RedisServiceExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connection);
            });

            services.AddScoped(typeof(IBasketRepo), typeof(BasketRep));

            return services;
        }
    }
}
