using System.Collections.Generic;

namespace Cryptocop.Software.API.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        // Nav props
        public User user { get; set; }
        public List<ShoppingCartItem> ShoppingCartItem { get; set; }
    }
}