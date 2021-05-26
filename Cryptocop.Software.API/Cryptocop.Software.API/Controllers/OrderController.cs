using Microsoft.AspNetCore.Mvc;
using Cryptocop.Software.API.Models;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using System;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Connect to service
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //  Gets all orders associated with the authenticated user
        [HttpGet]
        [Route("")]
        public IActionResult GetOrders()
        {
            return Ok(_orderService.GetOrders(User.Identity.Name));
        }

        //  Adds a new order associated with the authenticated user
        [HttpPost]
        [Route("")]
        public IActionResult CreateNewOrder([FromBody] OrderInputModel orderInfo)
        {
            if (!ModelState.IsValid) { return StatusCode(412, orderInfo); }
            return Ok(_orderService.CreateNewOrder(User.Identity.Name, orderInfo));
        }
    }
}