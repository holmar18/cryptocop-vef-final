using System;

namespace Cryptocop.Software.API.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ShoppingCardId { get; set; }
        public string ProductIdentifier { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }

        // Nav props
        public ShoppingCart ShoppingCart { get; set; }
    }
}