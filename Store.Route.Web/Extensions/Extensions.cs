using Microsoft.AspNetCore.Mvc;
using Store.Route.Domain.Contracts;
using Store.Route.Persistance;
using Store.Route.Services;
using Store.Route.Shared.ErrorModel;
using Store.Route.Web.Middlewares;
using System.Runtime.CompilerServices;

namespace Store.Route.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddBuiltInServices();
            services.AddSwaggerServices();
            services.AddInfrastructureServicesRegistration(configuration);
            services.AddApplicationServices(configuration);

            services.ConfigureServices();

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;


        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext =>
                {
                   var errors= actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                           .Select(m => new ValidationError()
                                           {
                                               Field = m.Key,
                                               Errors = m.Value.Errors.Select(m=>m.ErrorMessage)
                                           });

                    var Response = new ValidationErrorResponse() { Errors = errors };

                    return new BadRequestObjectResult(Response);
                });
            });

            return services;
        }


        public static async Task<WebApplication> ConfigureMiddleWares(this WebApplication app)
        {

           await app.InitializeDatabaseAsync();


            app.UseGlobalErrorHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            return app;
        
        }
    
         private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {   using var Scope=app.Services.CreateScope();

           var DbInitializer=  Scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.InitializeAsync();
            
            return app;
        }
    
         private static  WebApplication UseGlobalErrorHandling(this  WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }
    }
}
