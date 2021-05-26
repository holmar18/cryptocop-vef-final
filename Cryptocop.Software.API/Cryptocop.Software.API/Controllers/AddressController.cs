using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cryptocop.Software.API.Services.Interfaces;
using System;
using Cryptocop.Software.API.Models;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        // Connect to service
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }


        //  Gets all addresses associated with authenticated user
        [HttpGet]
        [Route("")]
        public IActionResult GetAllAddresses()
        {
            return Ok(_addressService.GetAllAddresses(User.Identity.Name));
        }

        //  Adds a new address associated with authenticated use
        [HttpPost]
        [Route("")]
        public IActionResult POSTAddress([FromBody] AddressInputModel addrModl)
        {
            if (!ModelState.IsValid) { return StatusCode(412, addrModl); }
            _addressService.AddAddress(User.Identity.Name, addrModl);
            return Ok();
        }

        //  Deletes an address by id
        [HttpDelete]
        [Route("{addressId:int}")]
        public IActionResult deleteAddressById(int addressId)
        {
            _addressService.DeleteAddress(User.Identity.Name, addressId);
            return NoContent();
        }
    }
}