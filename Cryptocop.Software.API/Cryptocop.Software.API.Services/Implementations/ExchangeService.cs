using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        public class ExchangeObjc
        {
            public string Id { get; set; }
            public string exchange_name { get; set; }
            public string exchange_slug { get; set; }
            public string pair { get; set; }         // Það eru 2 assets en þær eru settar saman i pair t.d BTC-USD
            public float? price_usd { get; set; }
            public DateTime? last_trade_at { get; set; }
        }
        static readonly HttpClient Client = new HttpClient();
        
        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            HttpResponseMessage response = await Client.GetAsync("https://data.messari.io/api/v1/markets?page=" + pageNumber.ToString());
            response.EnsureSuccessStatusCode();
        
            // des and get the data part of the response
            var data = await HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeObjc>(response);

            // Get the valuse we want and create Dto then add each to a list
            List<ExchangeDto> Storage = new List<ExchangeDto>();
            foreach(var item in data)
            {
                var singleData = new ExchangeDto 
                {
                    Id = item.Id.ToString(),
                    Name = item.exchange_name.ToString(),
                    Slug = item.exchange_slug.ToString(),
                    AssetSymbo = item.pair.ToString(),
                    PriceInUsd = item.price_usd,
                    LastTrade = item.last_trade_at,
                };
                Storage.Add(singleData);

            }
            IEnumerable<ExchangeDto> en = Storage;
            // Create envelope
            var env = new Envelope<ExchangeDto>(pageNumber, en);
            return env;
        }
    }
}