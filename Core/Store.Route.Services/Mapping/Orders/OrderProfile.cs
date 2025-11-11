using AutoMapper;
using Store.Route.Domain.Entities.Orders;
using Store.Route.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Mapping.Orders
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponse>()
                .ForMember(d=>d.DeliveryMethod,s=>s.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.Total,s=>s.MapFrom(s=>s.GetTotal()))
                 .ForMember(d => d.OrderAddress, s => s.MapFrom(s => s.ShippingAddress));
            ;


            CreateMap<OrderAddressDto, OrderAddress>().ReverseMap();


            CreateMap<OrderItem, OrderItemDto>()
                 .ForMember(s=>s.ProductId,o=>o.MapFrom(d=>d.Product.ProductId))
                  .ForMember(s=>s.ProductName,o=>o.MapFrom(d=>d.Product.ProductName))
                   .ForMember(s=>s.PictureUrl,o=>o.MapFrom(d=>d.Product.PictureUrl));

            CreateMap<DeliveryMethod,DeliveryMethodResponse>();
        }
    }
}
