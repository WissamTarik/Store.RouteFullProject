using AutoMapper;
using Store.Route.Domain.Contracts;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Abstractions.Basket;
using Store.Route.Services.Abstractions.Products;
using Store.Route.Services.Basket;
using Store.Route.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork,ICacheRepository _cacheRepository,IMapper _mapper,IBasketRepository _basketRepository) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork,_mapper);

        public IBasketServices BasketServices { get; } = new BasketServices(_basketRepository,_mapper);

        public ICacheService CacheService { get; }=new CacheService(_cacheRepository);
    }
}
