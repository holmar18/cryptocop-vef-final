using System.Collections.Generic;
using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<OrderDto> GetOrders(string email);
        OrderDto CreateNewOrder(string email, OrderInputModel order);
    }
}