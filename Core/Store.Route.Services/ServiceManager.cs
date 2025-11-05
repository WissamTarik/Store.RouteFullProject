using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Identity;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Abstractions.Auth;
using Store.Route.Services.Abstractions.Basket;
using Store.Route.Services.Abstractions.Order;
using Store.Route.Services.Abstractions.Products;
using Store.Route.Services.Auth;
using Store.Route.Services.Basket;
using Store.Route.Services.Orders;
using Store.Route.Services.Products;
using Store.Route.Shared.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork,
        ICacheRepository _cacheRepository,
        IMapper _mapper,
        IBasketRepository _basketRepository,
        UserManager<AppUser> _userManager,
        IOptions<JWTOptions> options
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(_unitOfWork,_mapper);

        public IBasketServices BasketServices { get; } = new BasketServices(_basketRepository,_mapper);

        public ICacheService CacheService { get; }=new CacheService(_cacheRepository);

        public IAuthService AuthService { get; } = new AuthService(_userManager,options);

        public IOrderService OrderService { get; } = new OrderService(_unitOfWork,_basketRepository,_mapper) ;
    }
}
