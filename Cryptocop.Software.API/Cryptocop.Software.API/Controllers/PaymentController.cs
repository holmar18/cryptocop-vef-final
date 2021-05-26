using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Models;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        // Connect to service
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        //  Gets all payment cards associated with the authenticated user
        [HttpGet]
        [Route("")]
        public IActionResult GetAllPayments()
        {
            return Ok(_paymentService.GetStoredPaymentCards(User.Identity.Name));
        }

        //  Adds a new payment card associated with the authenticated user
        [HttpPost]
        [Route("")]
        public IActionResult PostPayment([FromBody] PaymentCardInputModel cardInfo)
        {
            // if model is not valid return 412
            if (!ModelState.IsValid) { return StatusCode(412, cardInfo); }
            // send data to serv
            _paymentService.AddPaymentCard(User.Identity.Name, cardInfo);
            return NoContent();
        }
    }
}