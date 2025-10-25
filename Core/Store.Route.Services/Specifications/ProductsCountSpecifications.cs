using Store.Route.Domain.Entities.Products;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Specifications
{
    public class ProductsCountSpecifications : BaseSpecification<int,Product>
    {
        public ProductsCountSpecifications(ProductQueryParameters parameters) : base(
            p=>(!parameters.BrandId.HasValue || p.BrandId==parameters.BrandId)
             &&(!parameters.TypeId.HasValue||p.TypeId==parameters.TypeId)
            &&(string.IsNullOrEmpty(parameters.Search) ||p.Name.ToLower().Contains(parameters.Search.ToLower()) )
            
            
            )
        {
        }
    }
}
