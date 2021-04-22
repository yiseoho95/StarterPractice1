using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models.Request;
using ApplicationCore.ServiceInterfaces;

namespace StarterPractice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AccountController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Check Inputted Data");
            }

            var registeredUser = await _userService.RegisterUser(requestModel);
            return Ok(registeredUser);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUser")]
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserDetails(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> EmailExists([FromQuery] string email)
        {
            var user = await _userService.GetUser(email);
            return Ok(user == null ? new { emailExists = false } : new { emailExists = true });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel loginRequest)
        {
            var user = await _userService.ValidateUser(loginRequest.Email, loginRequest.Password);
            if (user == null)
                return Unauthorized();

            var JwtToken = _jwtService.GenerateToken(user);
            //must generate a JWT token and send it to client
            return Ok(new
            { token = JwtToken });
        }

    }
}
