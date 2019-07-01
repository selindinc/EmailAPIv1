using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmailApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            return StatusCode((int)HttpStatusCode.OK, Environment.MachineName);
        }
    }
}
