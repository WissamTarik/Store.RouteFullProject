using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Shared.Dtos.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderAddressDto OrderAddress { get; set; }
        public string DeliveryMethod { get; set; }//Delivery Method Name
        public ICollection<OrderItemDto> Items { get; set; }
        public decimal Subtotal { get; set; }//Price of items*quantity

        public decimal Total { get; set; }
    }
}
