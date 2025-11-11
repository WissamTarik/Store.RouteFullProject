using Store.Route.Domain.Entities;

namespace Store.Route.Domain.Entities.Orders
{
    //Table
    public class DeliveryMethod:BaseEntity<int>
    {
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Price { get; set; }
    }
}