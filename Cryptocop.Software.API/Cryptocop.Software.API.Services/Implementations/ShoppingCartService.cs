using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Models;
using System;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository = null, ICryptoCurrencyService cryptoCurrencyService = null)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _cryptoCurrencyService = cryptoCurrencyService;
        }
        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public async Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
        {
            // Get the current price for input 
            var cryptoDataPrice = await _cryptoCurrencyService.GetSingleCryptocurrencies(shoppingCartItemItem.ProductIdentifier);
            Console.Write(cryptoDataPrice.PriceInUsd);
            _shoppingCartRepository.AddCartItem(email, shoppingCartItemItem, cryptoDataPrice.PriceInUsd);
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingCartRepository.RemoveCartItem(email, id);
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
        }

        public void DeleteCart(string email)
        {
            _shoppingCartRepository.DeleteCart(email);
        }
    }
}
