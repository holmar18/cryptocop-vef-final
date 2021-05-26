using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Interfaces;


namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // Connect to service
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        //Registers a user within the application
        [AllowAnonymous]  // Allow unothorized
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterInputModel registerClient)
        {
            // if the input model is incorrect
            Console.Write("REGISTER");
            if (!ModelState.IsValid) { return StatusCode(412, registerClient); }
            UserDto data  = _accountService.CreateUser(registerClient);
            // if Null the Account did exist
            if (data == null) { return StatusCode(409); }
            // return the jwtToken
            return Ok(_tokenService.GenerateJwtToken(data));
        }

        // Signs the user in by checking the credentials provided and issuing a JWT token in return
        [AllowAnonymous]
        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] LoginInputModel loginClient)
        {
            if (!ModelState.IsValid) { return StatusCode(412, loginClient); }
            // call auth service
            var data = _accountService.AuthenticateUser(loginClient);
            if (data == null) { return Unauthorized(); }
            // return JWT token
            return Ok(_tokenService.GenerateJwtToken(data));
        }

        // Logs the user out by voiding the provided JWT token using the id found within the claim
        [HttpGet]
        [Route("signout")]
        public IActionResult SignOut([FromBody] LoginInputModel loginClient)
        {
            if (!ModelState.IsValid) { return StatusCode(412, loginClient); }
            int.TryParse(User.Claims.FirstOrDefault(r => r.Type == "tokenId").Value, out var tokenId);
            _accountService.Logout(tokenId);
            return NoContent();
        }


    }
}