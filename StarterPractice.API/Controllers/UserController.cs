using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;

namespace StarterPractice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}/purchases")]
        public async Task<IActionResult> GetPurchasedMovies()
        {
            // get all movies purchased by User
            // var userPurchasedMovies = _userService
            return Ok();
        }
    }
}