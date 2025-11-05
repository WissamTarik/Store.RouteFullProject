using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Route.Services.Abstractions;
using Store.Route.Services.Abstractions.Order;
using Store.Route.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrdersController(IServiceManager _serviceManager) :ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var result = await _serviceManager.OrderService.CreateOrder(request,userEmail);
            return Ok(result);

        }

        [HttpGet()]

        [Authorize]
        public async Task<IActionResult> GetOrdersOfSpecificUser()
        {
            var userEmail=User.FindFirst(ClaimTypes.Email).Value;
            var result = await _serviceManager.OrderService.GetOrdersForSpecificUser(userEmail);

            return Ok(result);
        }

        [HttpGet("{id}")]

        [Authorize]
        public async Task<IActionResult> GetOrderByIdForSpecificUser(Guid id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email).Value;

           var result=await  _serviceManager.OrderService.GetOrderByIdForSpecificUser(id, userEmail);
            return Ok(result);
        }


    
    }
}
