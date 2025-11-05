using Store.Route.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Abstractions.Order
{
    public interface IOrderService
    {
       Task <OrderResponse?> CreateOrder(OrderRequest Request,string userEmail);

        Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethods();

      Task<OrderResponse?>  GetOrderByIdForSpecificUser(Guid orderId, string userEmail);

       Task<IEnumerable<OrderResponse>?> GetOrdersForSpecificUser(string userEmail);
    }
}
