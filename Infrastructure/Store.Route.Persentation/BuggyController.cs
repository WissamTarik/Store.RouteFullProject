using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {

        [HttpGet("notfound")]// baseUrl/api/Buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            return NotFound();//404
        }

        [HttpGet("servererror")]// baseUrl/api/Buggy/servererror
        public IActionResult GetServerErrorRequest()
        {

            throw new Exception();
        }

        [HttpGet("badrequest")]//baseUrl/api/Buggy/badrequest

        public IActionResult GetBadRequest()
        {

            return BadRequest();//400
        }

        [HttpGet("badrequest/{id}/{age}")]//baseUrl/api/Buggy/badrequest/ahmed/ahmed

        public IActionResult GetBadRequest(int id,int age) { //Validation error

            return BadRequest();//400
        
        }

        [HttpGet("unauthorized")]//baseUrl/api/Buggy/unauthorized
        public IActionResult GetUnAuthorizedRequest() { 
        
          return Unauthorized();//401
        }
    }
}