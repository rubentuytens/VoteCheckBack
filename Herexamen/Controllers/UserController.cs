using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Herexamen.Models;
using Herexamen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Herexamen.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            System.Diagnostics.Debug.WriteLine(userParam);
            var user = _userService.Authenticate(userParam.UserName, userParam.Password);
            if (user == null )
                return BadRequest(new { message = user });
            user.Password = null;
            return Ok(user);
        }


    }
}