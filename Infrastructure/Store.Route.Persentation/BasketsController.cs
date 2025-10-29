using Microsoft.AspNetCore.Mvc;
using Store.Route.Services.Abstractions;
using Store.Route.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager _serviceManager):ControllerBase
    {
        [HttpGet]//api/Baskets?id=1k
        public async Task<IActionResult> GetBasketById(string id)
        {
            var Result= await _serviceManager.BasketServices.GetBasketAsync(id);

            return Ok(Result);

        }
        [HttpPost] //api/Baskets
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var Result = await _serviceManager.BasketServices.UpdateBasketAsync(basketDto);

            return Ok(Result);

        }
        [HttpDelete]//api/Basket?id=kkk
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await _serviceManager.BasketServices.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
