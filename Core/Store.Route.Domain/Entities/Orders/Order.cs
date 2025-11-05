using Store.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Domain.Entities.Orders
{
    public class Order:BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
        }

        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        public OrderAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }//Navigational Property

        public int DeliveryMethodId { get; set; }//FK

        public ICollection<OrderItem> Items { get; set; }//Navigational property

        public decimal Subtotal { get; set; }//Price of items*Quantity

        //[NotMapped]
        //public decimal Total { get; set; }//Subtotal + delivery method cost


        public decimal GetTotal() => Subtotal * DeliveryMethod.Price;//Not mapped
    }
}
