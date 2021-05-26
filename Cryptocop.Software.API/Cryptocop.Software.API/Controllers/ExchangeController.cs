using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Implementations;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/exchanges")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        // Connect to service
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        //  Gets all exchanges in a paginated envelope. This routes accepts a single query parameter called pageNumber which is used to paginate the results
        [HttpGet]
        [Route("/api/exchanges")]
        public IActionResult Exchanges([FromQuery] int pageNumber)
        {
            return Ok(_exchangeService.GetExchanges(pageNumber).Result);
        }
    }
}