using Store.Route.Shared;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Abstractions.Products
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters);

        Task<ProductResponse> GetProductByIdAsync(int id);

        Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync();
        Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync();
    }
}
