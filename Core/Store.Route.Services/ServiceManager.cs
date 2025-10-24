using AutoMapper;
using Store.Route.Domain.Contracts;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Abstractions.Products;
using Store.Route.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork,IMapper _mapper) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork,_mapper);
    }
}
