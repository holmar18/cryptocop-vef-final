using System.Data;
using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Models;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using System;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        // Connect to service
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // EXTRA
        //  Adds an item to the shopping cart, see Models section for reference
        [HttpGet]
        [Route("")]
        public IActionResult GetCartItems()
        {
            var data = _shoppingCartService.GetCartItems(User.Identity.Name);
            if (data == null) { return StatusCode(404); }
            return Ok(data);
        }


        //  Adds an item to the shopping cart, see Models section for reference
        [HttpPost]
        [Route("")]
        public IActionResult AddCartItem([FromBody] ShoppingCartItemInputModel item)
        {
            var data = _shoppingCartService.AddCartItem(User.Identity.Name, item);
            // wait for the thread to finish
            data.Wait();
            return Ok();
        }

        // Deletes an item from the shopping cart
        [HttpDelete]
        [Route("{itemId:int}", Name = "RemoveCartItem")]
        public IActionResult RemoveCartItem(int itemId)
        {
            Console.Write("Start item ID: " + itemId);
            _shoppingCartService.RemoveCartItem(User.Identity.Name, itemId);
            return NoContent();
        }

        // Updates the quantity for a shopping cart item
        [HttpPatch]
        [Route("{itemId:int}", Name = "UpdateCartItemQuantity")]
        public IActionResult UpdateCartItemQuantity(int itemId, [FromBody] ShoppingCartItemInputModel quantity)
        {
            _shoppingCartService.UpdateCartItemQuantity(User.Identity.Name, itemId, (float) quantity.Quantity);
           return NoContent();
        }

        // Clears the cart - all items within the cart should be deleted
        [HttpDelete]
        [Route("", Name = "clearCart")]
        public IActionResult clearCart()
        {
            _shoppingCartService.ClearCart(User.Identity.Name);
            return NoContent();
        }

        // ! Extra to delete the cart
        [HttpDelete]
        [Route("mine", Name = "DeleteCart")]
        public IActionResult DeleteCart()
        {
            _shoppingCartService.DeleteCart(User.Identity.Name);
            return NoContent();
        }
    }
}