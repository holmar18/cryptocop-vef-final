using System;
using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;

namespace Cryptocop.Software.API.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName   { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode   { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CardholderName   { get; set; }
        public string CreditCard { get; set; }
        public DateTime OrderDate { get; set; } // Represented as 01.01.2020   string not date
        public float TotalPrice   { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }  //OrderItems (list of OrderItemDto)
    }
}

