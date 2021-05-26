namespace Cryptocop.Software.API.Models
{
    public class CryptoCurrencyDto
    {
        public string Id { get; set; }     // Þetta á að vera int en id sem kemur af hinum api er string id
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public float PriceInUsd { get; set; }
        public string ProjectDetails { get; set; }
    }
}

