using System;
using System.Collections.Generic;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Repositories.Context;
using System.Linq;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Models.Dtos;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptoCopDbContext _dbContext;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private static readonly PaymentCardHelper _cardhelp = new PaymentCardHelper();

        public OrderRepository(CryptoCopDbContext dbContext, IShoppingCartRepository shoppingCartRepository = null)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
        }

        private Order ToOrder(User user, Address Address, PaymentCard card, float totalPrice, List<OrderItem> OrderItemss, int newId)
        {
            DateTime today = DateTime.Today;
            return new Order 
            {
                Id = newId,
                OrderId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                StreetName = Address.StreetName,
                HouseNumber = Address.HouseNumber,
                ZipCode = Address.ZipCode,
                Country = Address.Country,
                City = Address.City,
                CardHolderName = card.CardholderName,
                MaskedCreditCard = _cardhelp.MaskPaymentCard(card.CardNumber),
                OrderDate = new DateTime(today.Year, today.Month, today.Day, 10, 39, 30),
                TotalPrice = totalPrice,
                OrderItem = OrderItemss
            };
        }

        private OrderDto ToOrderDto(Order n, PaymentCard card, List<OrderItemDto> orderitem)
        {
            return new OrderDto 
            {
                Id = n.Id,
                Email = n.Email,
                FullName = n.FullName,
                StreetName = n.StreetName,
                HouseNumber = n.HouseNumber,
                ZipCode = n.ZipCode,
                Country = n.Country,
                City = n.City,
                CardholderName = n.CardHolderName,
                CreditCard = card.CardNumber,
                OrderDate = n.OrderDate,
                TotalPrice = n.TotalPrice,
                OrderItems = orderitem,
            };
        }

        private static OrderDto ToOrderDtoo(Order n, List<OrderItemDto> item)
        {

            return new OrderDto 
            {
                Id = n.Id,
                Email = n.Email,
                FullName = n.FullName,
                StreetName = n.StreetName,
                HouseNumber = n.HouseNumber,
                ZipCode = n.ZipCode,
                Country = n.Country,
                City = n.City,
                CardholderName = n.CardHolderName,
                CreditCard = n.MaskedCreditCard,
                OrderDate = n.OrderDate,
                TotalPrice = n.TotalPrice,
                OrderItems = item,
            };
        }

        private List<OrderItemDto> ordItem(List<OrderItem> eachItemList)
        {
            List<OrderItemDto> cartItems = new List<OrderItemDto>();
            // Create a  new order id
            foreach(var item in eachItemList)
            {
                var converItem = new OrderItemDto
                {
                    Id = item.Id,
                    ProductIdentifier = item.ProductIdentifier,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                };
                cartItems.Add(converItem);
            }
            return cartItems;
        }

        private static OrderItemDto ordItemDto(OrderItem item)
        {
                return new OrderItemDto
                {
                    Id = item.Id,
                    ProductIdentifier = item.ProductIdentifier,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                };
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            Console.Write(user.Email);
            // find all the Orders with his ID
            List<OrderItemDto> item = new List<OrderItemDto>();
            var data = _dbContext.Order.Where(m => m.OrderId == user.Id).Select(a => ToOrderDtoo(a, item)).ToList();
            if (data == null) { return null; }
            // ! TODO : Ná i öll order item, loop data og filtera fyrri partin eftir Order id og bæta i OrderItem LIstan sem hver order á
            // Find all OrderItems
            //var ordItem = _dbContext.OrderItem.Select(m => m).ToArray();
            // Console.Write("COUNT: " + ordItem.Count());
            // if (ordItem.Count() == 0) { return null; }
            // for(var i=1; i < ordItem.Count(); i++)
            // {
            //      Console.Write(ordItem[i].OrderId);
            //      Console.Write(ordItem[i].ProductIdentifier);
            // }
            // List<OrderDto> retVal = new List<OrderDto>();
            // // Loop and filter each orders item
            // foreach(var item in data)
            // {
            //     //List<OrderItemDto> filteredList = ordItem.Where( x => x.OrderId == item.Id).Select(a => ordItemDto(a)).ToList();
            //     List<OrderItemDto> filteredList = new List<OrderItemDto>();
            //     var re = ToOrderDtoo(item, filteredList);
            //     retVal.Add(re);
            // }
            
            IEnumerable<OrderDto> n = data;
            return n;
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            // Get user info with email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // Find the address with USER ID from prev
            var address = _dbContext.Address.FirstOrDefault(q => q.UserId == user.Id);
            // Get the Payment card with the card ID from Order input model
            var card = _dbContext.PaymentCard.FirstOrDefault(q => q.UserId == user.Id);
            // get the cart items
            var cartItem = _shoppingCartRepository.GetCartItems(email);
            List<OrderItem> cartItemDto = new List<OrderItem>();
            // get the total price & create a new list of converted ShopingItem to OrderItemDto
            float totalPrice = 0;
            // Create a  new order id
            var newId = _dbContext.Order.Count() + 1;
            foreach(var item in cartItem)
            {
                var converItem = new OrderItem
                {
                    Id = item.Id,
                    OrderId = newId,
                    ProductIdentifier = item.ProductIdentifier,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                };
                cartItemDto.Add(converItem);
                totalPrice += item.TotalPrice;
            }
            // Create order where digits on card are masked that goes to the data base
            var orderToSave = ToOrder(user, address, card, totalPrice, cartItemDto, newId);
            _dbContext.Order.Add(orderToSave);
            _dbContext.SaveChanges();


            // Create Order and same with OrderItemDto and return it
            var cartItems = ordItem(cartItemDto);

            var retVal = ToOrderDto(orderToSave, card, cartItems);

            // Delete items from cart
            _shoppingCartRepository.ClearCart(email);
            // delete cart
            _shoppingCartRepository.DeleteCart(email);

            return retVal;
        }
    }
}