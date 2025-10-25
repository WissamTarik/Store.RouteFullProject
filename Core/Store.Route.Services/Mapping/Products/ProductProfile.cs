using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Route.Domain.Entities.Products;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Mapping.Products
{
    public class ProductProfile:Profile
    {
        public ProductProfile(IConfiguration configuration )
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(d => d.Brand, s => s.MapFrom(s => s.Brand.Name))
                 .ForMember(d => d.Type, s => s.MapFrom(s => s.Type.Name))
                 //.ForMember(d => d.PictureUrl, s => s.MapFrom(s => $"{configuration["BaseUrl"]}/{s.PictureUrl}"))
                 .ForMember(d => d.PictureUrl, s => s.MapFrom(new ProductPictureUrlResolver(configuration)))
                 ;


            CreateMap<ProductBrand, BrandTypeResponse>();

            CreateMap<ProductType, BrandTypeResponse>();
        }
    }
}
