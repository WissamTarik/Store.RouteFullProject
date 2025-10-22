using Store.Route.Domain.Entities.Products;
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

        public ProductsWithBrandAndTypeSpecifications(int? brandId, int? typeId,string? sort,string? search) : base(
           p=>(!brandId.HasValue || brandId==p.BrandId)
              &&
             (!typeId.HasValue || typeId == p.TypeId)
           &&
           (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search.ToLower()))
                )
        {

            ApplySorting(sort);

            ApplyIncludes();
        }
        public ProductsWithBrandAndTypeSpecifications(int id):base(p=>p.Id==id)
        {
            ApplyIncludes();
        }

        private void ApplySorting(string? sort)
        {
            if (string.IsNullOrEmpty(sort))
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
