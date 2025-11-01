using Microsoft.AspNetCore.Mvc;
using Store.Route.Services.Abstractions;
using Store.Route.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager):ControllerBase
    {

        [HttpPost("login")]//POST://api/Auth/login

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
         var Result=   await _serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(Result);
        }

        [HttpPost("register")]//POST://api/Auth/register

        public async Task<IActionResult> Register(RegisterDto registerDto) {

            var Result = await _serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(Result);
        
        }
    }
}
