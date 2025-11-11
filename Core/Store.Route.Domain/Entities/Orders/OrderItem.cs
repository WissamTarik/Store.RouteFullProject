using Store.Route.Domain.Entities;

namespace Store.Route.Domain.Entities.Orders
{
    //Table
    public class OrderItem:BaseEntity<int>
    {
        public OrderItem()
        {
            
        }

        public OrderItem(ProductInOrderItem product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductInOrderItem Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}