using System;
using System.Collections.Generic;

namespace Cryptocop.Software.API.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        // Navigational properties
        public User User { get; set; }
    }
}