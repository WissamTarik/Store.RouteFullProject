using AutoMapper;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Basket;
using Store.Route.Domain.Exceptions;
using Store.Route.Services.Abstractions.Basket;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Basket
{
    public class BasketServices(IBasketRepository _basketRepository,IMapper _mapper) : IBasketServices
    {
        public async Task<BasketDto?> GetBasketAsync(string id)
        {
            var Basket=await _basketRepository.GetBasketAsync(id);
            if (Basket is null) throw new BasketNotFoundException(id);

            var Result = _mapper.Map<BasketDto>(Basket);
            return Result;
        }

        public async Task<BasketDto?> UpdateBasketAsync(BasketDto basket)
        {
            var Result = _mapper.Map<CustomerBasket>(basket);
             Result=await  _basketRepository.UpdateBasketAsync(Result);
            if (Result is null) throw new BasketCreateOrUpdateBadRequestException();

            var Returned = _mapper.Map<BasketDto>(Result);
            return Returned;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
           var Flag= await _basketRepository.DeleteBasketAsync(id);
            if (!Flag) throw new  BasketDeleteBadRequest();
            return Flag;
        }

       
    }
}
