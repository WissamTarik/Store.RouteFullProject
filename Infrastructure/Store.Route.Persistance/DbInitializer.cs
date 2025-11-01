using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Identity;
using Store.Route.Domain.Entities.Products;
using Store.Route.Persistance.Data.Contexts;
using Store.Route.Persistance.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Route.Persistance
{
    public class DbInitializer(StoreDbContext _context,
        StoreIdentityDbContext IdentityDbContext,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IDbInitializer
    {
        private readonly StoreIdentityDbContext _IdentityDbContext = IdentityDbContext;
        private readonly UserManager<AppUser> _UserManager = userManager;
        private readonly RoleManager<IdentityRole> _RoleManager = roleManager;

        public async Task InitializeAsync()
        {
            //Create DB
            //Update DB
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
               await _context.Database.MigrateAsync();
            }

            //Data Seeding

            #region Data seeding of product brand
            //ProductBrand

            //1.Read all data from json file "brands.json"
            //Infrastructure\Store.Route.Persistance\Data\DataSeeding\brands.json
            if (!_context.ProductBrands.Any())
            {
                var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Route.Persistance\Data\DataSeeding\brands.json");

                //2.Convert  jsonString to List<ProductBrand>

                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                //3.Add brands list to DB

                if (Brands is not null && Brands.Count() > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(Brands);
                }

            }
            #endregion


            #region Data seeding of Product type
            if (!_context.ProductTypes.Any())
            {
                //1.Read all data from types.json

             var TypesData=   await File.ReadAllTextAsync(@"..\Infrastructure\Store.Route.Persistance\Data\DataSeeding\types.json");
            
                //2.Convert jsonString to ProductType List
            
                var Types=JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                if(Types is not null && Types.Count() > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(Types);
                }
            
            
            
            
            }
            #endregion



            #region Data seeding of Product
            if (!_context.Products.Any())
             {
                //1.Read all data from Products json

                var ProductsData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Route.Persistance\Data\DataSeeding\products.json");
              
                //2.Convert jsonString to List<Product>

                var Products=JsonSerializer.Deserialize<List<Product>>(ProductsData);


                //3.Add Products to DB
                if( Products is not null && Products.Count() > 0)
                {

                    await _context.Products.AddRangeAsync(Products);
                }
            
            }

            #endregion

            await _context.SaveChangesAsync();

        }

        public async Task InitializeIdentityAsync()
        {
            //Create Database if It does'nt exist && apply any pending migration
            if (_IdentityDbContext.Database.GetPendingMigrations().Any())
            {
              await  _IdentityDbContext.Database.MigrateAsync();
            }

            //Data seeding

            //Create Role
            if (!_RoleManager.Roles.Any())
            {

           await _RoleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            await _RoleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
            }
          
            
            if (!_UserManager.Users.Any())
            {
                var SuperAdmin = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789"
                };
                var Admin = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"
                };
               await _UserManager.CreateAsync(SuperAdmin, "P@ssW0rd");
                await _UserManager.CreateAsync(Admin, "P@ssW0rd");


               await _UserManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");

                await _UserManager.AddToRoleAsync(Admin, "Admin");
            }

           await _IdentityDbContext.SaveChangesAsync();
        }
    }
}
