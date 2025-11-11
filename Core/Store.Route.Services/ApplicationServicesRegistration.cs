using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Mapping.Basket;
using Store.Route.Services.Mapping.Orders;
using Store.Route.Services.Mapping.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            //services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile()));
            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        
        }
    }
}
