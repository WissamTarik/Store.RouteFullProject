
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Persistance;
using Store.Route.Persistance.Data.Contexts;
using Store.Route.Services;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Mapping.Products;
using Store.Route.Shared.ErrorModel;
using Store.Route.Web.Extensions;
using Store.Route.Web.Middlewares;
using System.Threading.Tasks;

namespace Store.Route.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            //builder.Services.AddDbContext<StoreDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});
            //builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            //builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //builder.Services.AddScoped<IServiceManager,ServiceManager>();

            //builder.Services.AddInfrastructureServicesRegistration(builder.Configuration);
            ////builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile(builder.Configuration)));

            //builder.Services.AddApplicationServices(builder.Configuration);




            //builder.Services.Configure<ApiBehaviorOptions>(config =>
            //{
            //    config.InvalidModelStateResponseFactory = (actionContext) =>
            //    {
            //        var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
            //                    .Select(m => new ValidationError()
            //                    {
            //                        Field = m.Key,
            //                        Errors = m.Value.Errors.Select(e=>e.ErrorMessage)
            //                    });

            //        var Response = new ValidationErrorResponse()
            //        {
            //            Errors = errors
            //        };
            //        return new BadRequestObjectResult(Response);
            //    };
            //});

            builder.Services.RegisterAllServices(builder.Configuration);
            var app = builder.Build();

           await app.ConfigureMiddleWares();

            app.Run();
        }
    }
}
