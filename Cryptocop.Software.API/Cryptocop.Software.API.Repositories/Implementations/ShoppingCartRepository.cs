using System.Collections.Generic;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Repositories.Context;
using System.Linq;
using System;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptoCopDbContext _dbContext;

        public ShoppingCartRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static ShoppingCartItemDto ToShopinCartItemrDto(ShoppingCartItem item)
        {
            return new ShoppingCartItemDto {
                Id = item.Id,
                ProductIdentifier = item.ProductIdentifier,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice,
            };
        }
        private static ShoppingCartItem ToShopinCartItemr(ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd, int newId, int cartId)
        {
            return new ShoppingCartItem {
                Id = newId,
                ShoppingCardId = cartId,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float)shoppingCartItemItem.Quantity,
                UnitPrice = priceInUsd,
            };
        }

        // Helper
        private void CreateNewCart(User data)
        {
            int newID = _dbContext.ShoppingCart.Count() + 1;
            var info = new ShoppingCart
            {
                Id = newID,
                UserId = data.Id
            };
            // add & save
            _dbContext.ShoppingCart.Add(info);
            _dbContext.SaveChanges();
        }

        // Helper
        private IQueryable<ShoppingCartItem> FindCartItem(string email)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // find the cart owned by that user
            var cart = _dbContext.ShoppingCart.FirstOrDefault(r => r.UserId == user.Id);
            if (cart == null) { return null; }
            // find all his cartitems
            var data = _dbContext.ShoppingCartItem.Where(m => m.ShoppingCardId == cart.Id).Select(m => m);
            if (data == null) { return null; }
            return data;
        }





        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // find the cart
            var cart = _dbContext.ShoppingCart.FirstOrDefault(r => r.UserId == user.Id);
            // if there is no cart create empty base
            if (cart == null) 
            {
                CreateNewCart(user);
                return null;
            }
            // Now search for the items
            var data = _dbContext.ShoppingCartItem.Where(m => m.ShoppingCardId == cart.Id).Select(m => ToShopinCartItemrDto(m));
            return data;
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // Create ShoppingCartItem and give quantity value 1 
            // Need new ID
            var cart = _dbContext.ShoppingCart.FirstOrDefault(r => r.UserId == user.Id);  // find the cart
            // if no cart create one
            if (cart == null) 
            {
                CreateNewCart(user);
                // find the new cart
                cart = _dbContext.ShoppingCart.FirstOrDefault(r => r.UserId == user.Id);
            }
            int newId = _dbContext.ShoppingCartItem.Count() + 1;
            var newData = ToShopinCartItemr(shoppingCartItemItem, priceInUsd, newId, cart.Id);
            Console.Write(newData);
            // Add & Save
            _dbContext.ShoppingCartItem.Add(newData);
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int id)
        {
            // find all of users cart items if zero return
            var data = FindCartItem(email);
            if (data == null) { return; }
            // find the item to delete if zero return
            var itemToRm = data.FirstOrDefault(r => r.Id == id);
            if (itemToRm == null) { return; }
            _dbContext.ShoppingCartItem.Remove(itemToRm);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            // find all users cartitems
            var data = FindCartItem(email);
            if (data == null) { return; }
            // find the specific one
            var itemToUpd = data.FirstOrDefault(r => r.Id == id);
            if (itemToUpd == null) { return; }
            // Update & save
            itemToUpd.Quantity = quantity;
            _dbContext.SaveChanges();
        }

        public void ClearCart(string email)
        {
            // find all users cartitems
            var data = FindCartItem(email);
            // Remove all items
            foreach(var item in data)
            {
                _dbContext.ShoppingCartItem.Remove(item);
            }
            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        {
            // Delete all items from the cart first
            ClearCart(email);
            // find the user with the email
            var user = _dbContext.User.FirstOrDefault(r => r.Email == email);
            // find the cart owned by that user
            var cart = _dbContext.ShoppingCart.FirstOrDefault(r => r.UserId == user.Id);
            // delete the cart & save
            _dbContext.ShoppingCart.Remove(cart);
            _dbContext.SaveChanges();
        }
    }
}