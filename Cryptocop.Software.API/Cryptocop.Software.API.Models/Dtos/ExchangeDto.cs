using System;

namespace Cryptocop.Software.API.Models
{
    public class ExchangeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string AssetSymbo { get; set; }
        public float? PriceInUsd { get; set; }
        public DateTime? LastTrade { get; set; }
    }
}

