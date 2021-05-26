using System.Collections.Generic;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IQueueService _queueService;

        public OrderService(IOrderRepository orderRepository, IQueueService queueService = null)
        {
            _orderRepository = orderRepository;
            _queueService = queueService;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            return _orderRepository.GetOrders(email);
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            OrderDto info = _orderRepository.CreateNewOrder(email, order);
            _queueService.PublishMessage("create-order", info, "email-thanks");
            return info;
        }
    }
}