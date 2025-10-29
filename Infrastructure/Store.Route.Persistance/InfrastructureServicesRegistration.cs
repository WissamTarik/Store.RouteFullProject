using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Store.Route.Domain.Contracts;
using Store.Route.Persistance.Data.Contexts;
using Store.Route.Persistance.Repositories;
using Store.Route.Services;
using Store.Route.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persistance
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServicesRegistration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<ICacheRepository,CacheRepository>();
            services.AddScoped<ICacheService,CacheService>();

            services.AddTransient<IConnectionMultiplexer>((serviceProvider) =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            });
            return services;
        }
    }
}
