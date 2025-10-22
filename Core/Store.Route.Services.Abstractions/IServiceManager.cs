using Store.Route.Services.Abstractions.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Abstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
    }
}
