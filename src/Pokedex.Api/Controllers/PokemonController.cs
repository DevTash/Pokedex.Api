using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Pokedex.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PokemonController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pokedex");
        }
    }
}
