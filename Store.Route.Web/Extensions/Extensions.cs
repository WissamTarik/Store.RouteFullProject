using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Identity;
using Store.Route.Persistance;
using Store.Route.Persistance.Data.Contexts;
using Store.Route.Persistance.Identity;
using Store.Route.Services;
using Store.Route.Shared.ErrorModel;
using Store.Route.Web.Middlewares;
using System.Runtime.CompilerServices;
using Microsoft.IdentityModel.Tokens;
using Store.Route.Shared.JWT;
using System.Text;
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
            services.AddIdentityServices();
            services.ConfigureServices();
            services.ConfigureJWTServices(configuration);
            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;


        }
       

        private static IServiceCollection ConfigureJWTServices(this IServiceCollection services,IConfiguration configuration)
        {
            var JwtOptions = configuration.GetSection("JwtOptions").Get<JWTOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience=true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    
                    ValidIssuer=JwtOptions.Issuer,
                    ValidAudience=JwtOptions.Audience,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey)),
                };
            });


            return services;
        }


        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                   .AddEntityFrameworkStores<StoreIdentityDbContext>();

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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            return app;
        
        }
    
         private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {   using var Scope=app.Services.CreateScope();

           var DbInitializer=  Scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.InitializeAsync();
            await DbInitializer.InitializeIdentityAsync();
            return app;
        }
    
         private static  WebApplication UseGlobalErrorHandling(this  WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }
    }
}
