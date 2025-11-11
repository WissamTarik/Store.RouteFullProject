using AutoMapper;
using Store.Route.Domain.Contracts;
using Store.Route.Domain.Entities.Orders;
using Store.Route.Domain.Entities.Products;
using Store.Route.Domain.Exceptions;
using Store.Route.Services.Abstractions.Order;
using Store.Route.Services.Specifications.Orders;
using Store.Route.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Services.Orders
{
    public class OrderService(IUnitOfWork _unitOfWork,IBasketRepository _basketRepository,IMapper _mapper) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrder(OrderRequest Request, string userEmail)
        {
            //1.Get order Address
            var shippingAddress = _mapper.Map<OrderAddress>(Request.ShipToAddress);
           
            //2.Get delivery method
                 
            var deliveryMethod=await _unitOfWork.GetRepository<int,DeliveryMethod>().GetAsync(Request.DeliveryMethodId);

            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(Request.DeliveryMethodId);

            //3.Get orders item
              //3.Get basket item
            var Basket=await  _basketRepository.GetBasketAsync(Request.BasketId);

            if (Basket is null) throw new BasketNotFoundException(Request.BasketId);

            //3.2 convert all basket items to order items
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in Basket.Items) {

                //3.3 check compatibility of price between item and product in database
                var product=await _unitOfWork.GetRepository<int,Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundException(item.Id);
                if (product.Price != item.Price) { item.Price = product.Price; }
                var productInItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInItem,item.Price, item.Quantity);
                orderItems.Add(orderItem);
                
            }

            //4.Calculate subtotal

            var subtotal = orderItems.Sum(oi => oi.Price * oi.Quantity);
            //Create Order

            var order = new Order(userEmail,shippingAddress,deliveryMethod,orderItems,subtotal);

            //Add order to database
            await _unitOfWork.GetRepository<Guid, Order>().AddAsync(order);
            var Count= await _unitOfWork.SaveChangesAsync();
            if (Count <= 0) throw new CreateOrderBadRequest();
            var result= _mapper.Map<OrderResponse>(order);
            return result;
        }

        //public async Task<OrderResponse?> CreateOrder(OrderRequest Request, string userEmail)
        //{

        //    //Get order address
        //   var OrderAddress=  _mapper.Map<OrderAddress>(Request.ShipToAddress);


        //    //Get delivery method
        //   var DeliveryMethod=  await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAsync(Request.DeliveryMethodId);

        //    if (DeliveryMethod is null) throw new DeliveryMethodNotFoundException(Request.DeliveryMethodId);


        //    //Get items 
        //    //1.Get basket by id
        //    var Basket=await _basketRepository.GetBasketAsync(Request.BasketId);

        //    if (Basket is null) throw new BasketNotFoundException(Request.BasketId);

        //    //2.Convert Every basket item to order item

        //    List<OrderItem> orderItems = new List<OrderItem>();
        //    foreach (var item in Basket.Items)
        //    {
        //        var product = await _unitOfWork.GetRepository<int, Product>().GetAsync(item.Id);

        //        if (product is null) throw new ProductNotFoundException(item.Id);
        //        if(product.Price!=item.Price)
        //        {
        //            item.Price = product.Price;
        //        }
        //        var productInOrderItem=new ProductInOrderItem(item.Id,item.ProductName,item.PictureUrl);
        //        var OrderItem = new OrderItem(productInOrderItem,item.Price,item.Quantity);
        //        orderItems.Add(OrderItem);
        //    }

        //    var Subtotal = orderItems.Sum(o => o.Price * o.Quantity);
        //    //Create order
        //    var Order = new Order(userEmail,OrderAddress, DeliveryMethod,orderItems,Subtotal);

        //    //Add order to DB
        //     await _unitOfWork.GetRepository<Guid, Order>().AddAsync(Order);

        //    var Count= await _unitOfWork.SaveChangesAsync();
        //    if (Count <= 0) throw new CreateOrderBadRequest();
        //    return _mapper.Map<OrderResponse>(Order);

        //}

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethods()
        {
           var DeliveryMethods=await _unitOfWork.GetRepository<int, DeliveryMethod>().GetAllAsync();

            if (DeliveryMethods is null) throw new BadRequestException("Invalid Operation");
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(DeliveryMethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUser(Guid orderId, string userEmail)
        {
            var spec=new OrderSpecification(orderId,userEmail);
            var Order = await _unitOfWork.GetRepository<Guid, Order>().GetAsync(spec, orderId); 
            return _mapper.Map<OrderResponse>(Order);
        }

        public async Task<IEnumerable<OrderResponse>?> GetOrdersForSpecificUser(string userEmail)
        {
            var spec = new OrderSpecification(userEmail);

            var orders=await _unitOfWork.GetRepository<Guid,Order>().GetAllAsync(spec);

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }
    
      
    }
}
