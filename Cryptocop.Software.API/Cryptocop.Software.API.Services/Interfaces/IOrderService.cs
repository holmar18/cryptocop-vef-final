using System.Collections.Generic;

using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetOrders(string email);
        OrderDto CreateNewOrder(string email, OrderInputModel order);
    }
}