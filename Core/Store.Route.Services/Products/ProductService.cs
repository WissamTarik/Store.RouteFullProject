using AutoMapper;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Products;
using Store.Route.Services.Abstractions.Products;
using Store.Route.Services.Specifications;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Products
{
    public class ProductService(IUnitOfWork _unitOfWork,IMapper _mapper) : IProductService
    {

        //priceasc
        //pricedsc
        //name
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync(int?brandId,int?typeId,string?sort,string ?search)
        {
            //var spec = new BaseSpecification<int, Product>(null);
            //spec.Includes.Add(p => p.Brand);
            //spec.Includes.Add(p => p.Type);


            var spec = new ProductsWithBrandAndTypeSpecifications(brandId,typeId,sort,search);
            var Products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync(spec);

            var Result = _mapper.Map<IEnumerable<ProductResponse>>(Products);
            return Result;
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            //var spec = new BaseSpecification<int, Product>(p => p.Id == id);
            //spec.Includes.Add(p => p.Brand);
            //spec.Includes.Add(p => p.Type);


            var spec=new ProductsWithBrandAndTypeSpecifications( id);
            var Product = await _unitOfWork.GetRepository<int, Product>().GetAsync(spec,id);
            var Result = _mapper.Map<ProductResponse>(Product);
            return Result;

        }
        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var Brands =  await _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();

            var Result = _mapper.Map<IEnumerable<BrandTypeResponse>>(Brands);
            return Result;
        
        }

      

        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync()
        {

            var Types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var Result = _mapper.Map<IEnumerable<BrandTypeResponse>>(Types);
            return Result;
        }

       
    }
}
