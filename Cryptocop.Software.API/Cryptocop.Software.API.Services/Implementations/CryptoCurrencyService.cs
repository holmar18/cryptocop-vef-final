using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using System;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        public class CryptoObj 
        { 
            public string id { get; set; } 
            public string symbol { get; set; } 
            public string name { get; set; } 
            public string slug { get; set; } 
            public string _internal_temp_agora_id { get; set; }  
            public Object metrics { get; set; } 
            public Object profile { get; set; } 
        }

        public class SingleCryptoObj 
        { 
            public string id { get; set; } 
            public string symbol { get; set; } 
            public string name { get; set; } 
            public string slug { get; set; } 
            public Object market_data { get; set; } 
        }
        static readonly HttpClient Client = new HttpClient();


        public async Task<IEnumerable<CryptoCurrencyDto>> GetAvailableCryptocurrencies()
        {
        HttpResponseMessage response = await Client.GetAsync("https://data.messari.io/api/v2/assets?with-metrics&with-profiles");
        response.EnsureSuccessStatusCode();
        
        // des and get the data part of the response
        var data = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptoObj>(response);

        // Get the valuse we want and create Dto then add each to a list
        List<CryptoCurrencyDto> Storage = new List<CryptoCurrencyDto>();
        foreach(var item in data)
        {
            var jsonObjectMetrics = JObject.Parse(item.metrics.ToString());
            var jsonObjectProfile = JObject.Parse(item.profile.ToString());

            var singleData = new CryptoCurrencyDto 
            {
                Id = item.id.ToString(),
                Symbol = item.symbol.ToString(),
                Name = item.name.ToString(),
                Slug = item.slug.ToString(),
                PriceInUsd = (float) ((double)(jsonObjectMetrics["market_data"]["price_usd"])),
                ProjectDetails = jsonObjectProfile["general"]["overview"]["project_details"].ToString(),
            };
            Storage.Add(singleData);
        }
        // Filter data BTC; ETH; USDT; XMR
        var results = Storage.Where(o => o.Symbol == "BTC" || o.Symbol == "ETH" || o.Symbol == "USDT" || o.Symbol == "XMR");
        // Create a copy of list in IEnum to return
        IEnumerable<CryptoCurrencyDto> en = results;
        return en;
        }


        public async Task<CryptoCurrencyDto> GetSingleCryptocurrencies(string symbol)
        {
            HttpResponseMessage response = await Client.GetAsync("https://data.messari.io/api/v1/assets/" + symbol + "/metrics/market-data");
            response.EnsureSuccessStatusCode();
            
            // des and get the data part of the response
            var data = await HttpResponseMessageExtensions.DeserializeJsonToObject<SingleCryptoObj>(response);
            // Get the valuse we want and create Dto then add each to a list
            var jsonObjectMetrics = JObject.Parse(data.market_data.ToString());
            var singleData = new CryptoCurrencyDto 
            {
                Id = data.id.ToString(),
                Symbol = data.symbol.ToString(),
                Name = data.name.ToString(),
                Slug = data.slug.ToString(),
                PriceInUsd = (float) ((double)(jsonObjectMetrics["price_usd"])),
                ProjectDetails = null,
            };
            return singleData;
        }
    }
}