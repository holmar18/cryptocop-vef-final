using System;
using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;

namespace Cryptocop.Software.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Extra Foreign Key Relation þetta er id af User
        public string Email { get; set; }
        public string FullName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CardHolderName { get; set; }
        public string MaskedCreditCard { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalPrice { get; set; }

        // Nav props
        public User User { get; set; }
        public List<OrderItem> OrderItem { get; set; }
    }
}