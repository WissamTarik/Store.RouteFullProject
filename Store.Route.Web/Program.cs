
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Persistance;
using Store.Route.Persistance.Data.Contexts;
using Store.Route.Services;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Mapping.Products;
using Store.Route.Shared.ErrorModel;
using Store.Route.Web.Middlewares;

namespace Store.Route.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IServiceManager,ServiceManager>();


            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile(builder.Configuration)));

            builder.Services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                .Select(m => new ValidationError()
                                {
                                    Field = m.Key,
                                    Errors = m.Value.Errors.Select(e=>e.ErrorMessage)
                                });

                    var Response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(Response);
                };
            });
            
            
            var app = builder.Build();


            var Scope = app.Services.CreateScope();

            var DbInitializer = Scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            DbInitializer.InitializeAsync();


            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

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

            app.Run();
        }
    }
}
