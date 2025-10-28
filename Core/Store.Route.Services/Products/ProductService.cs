using AutoMapper;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Products;
using Store.Route.Domain.Exceptions;
using Store.Route.Services.Abstractions.Products;
using Store.Route.Services.Specifications;
using Store.Route.Shared;
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
        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters)
        {
            //var spec = new BaseSpecification<int, Product>(null);
            //spec.Includes.Add(p => p.Brand);
            //spec.Includes.Add(p => p.Type);


            var spec = new ProductsWithBrandAndTypeSpecifications(parameters );
            var Products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync(spec);

            var Result = _mapper.Map<IEnumerable<ProductResponse>>(Products);
            var specCount = new ProductsCountSpecifications(parameters);
             var Count=  await _unitOfWork.GetRepository<int, Product>().CountAsync(specCount);
            PaginationResponse<ProductResponse> Response = new PaginationResponse<ProductResponse>(parameters.PageSize,parameters.PageIndex,Count,Result);
           
            return Response;
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            //var spec = new BaseSpecification<int, Product>(p => p.Id == id);
            //spec.Includes.Add(p => p.Brand);
            //spec.Includes.Add(p => p.Type);


            var spec=new ProductsWithBrandAndTypeSpecifications( id);
            var Product = await _unitOfWork.GetRepository<int, Product>().GetAsync(spec,id);

            if (Product is null) throw new ProductNotFoundException(id);
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
