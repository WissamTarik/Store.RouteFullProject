using Microsoft.EntityFrameworkCore;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Products;
using Store.Route.Persistance.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Route.Persistance
{
    public class DbInitializer(StoreDbContext _context) : IDbInitializer
    {
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
    }
}
