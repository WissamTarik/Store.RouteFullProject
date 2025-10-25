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
    public class ProductsWithBrandAndTypeSpecifications:BaseSpecification<int,Product>
    {

        public ProductsWithBrandAndTypeSpecifications(ProductQueryParameters  parameters) : base(
           p=>(!parameters.BrandId.HasValue|| parameters.BrandId==p.BrandId)
              &&
             (!parameters.TypeId.HasValue || parameters.TypeId == p.TypeId)
           &&
           (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search.ToLower()))
                )
        {

            //PageIndex=3
            //PageSize=5
            //Skip=(3-1)*5 (PageIndex-1)*PageSize
            //Take=5 PageSize
            ApplySorting(parameters.Sort);

            ApplyIncludes();
            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
        public ProductsWithBrandAndTypeSpecifications(int id):base(p=>p.Id==id)
        {
            ApplyIncludes();
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "pricedesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
        }


        private void ApplyIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }
    }
}
