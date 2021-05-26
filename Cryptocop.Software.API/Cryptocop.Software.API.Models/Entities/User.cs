using System.Collections.Generic;

namespace Cryptocop.Software.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Navigational properties
        public List<Address> Address { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public List<PaymentCard> PaymentCard { get; set; }
        public List<Order> Order { get; set; }
    }
}