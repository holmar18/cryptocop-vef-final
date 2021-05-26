using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        // Connect to service
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public CryptoCurrencyController(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        //  Gets all available cryptocurrencies - the only available cryptocurrencies in this platform (Only 4 available)
        [HttpGet]
        [Route("")]
        public IActionResult CryptoCurrencies()
        {
            var data = _cryptoCurrencyService.GetAvailableCryptocurrencies();
            return Ok(data.Result);
        }
    }
}
