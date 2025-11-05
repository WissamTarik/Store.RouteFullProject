using Store.Route.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Specifications.Orders
{
    public class OrderSpecification : BaseSpecification<Guid, Order>
    {
        public OrderSpecification(Guid id,string userEmail) : base(o=>o.Id==id && o.UserEmail.ToLower()==userEmail.ToLower())
        {
            ApplyIncludes();
            
        }
        public OrderSpecification(string userEmail):base(o=>userEmail.ToLower()==userEmail.ToLower())
        {
            ApplyIncludes();
            AddOrderByDescending(o=>o.OrderDate);
        }

        private void ApplyIncludes()
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
