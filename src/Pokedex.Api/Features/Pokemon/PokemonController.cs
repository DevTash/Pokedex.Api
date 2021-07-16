using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Pokedex.Api.Features.Pokemon
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService ?? throw new ArgumentNullException(nameof(pokemonService));
        }

        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> GetBasicInfo(string pokemonName)
        {
            try
            {
                var basicInfo = await _pokemonService.GetBasicInfoByNameAsync(pokemonName);

                if (basicInfo == null)
                {
                    return NotFound();
                }

                return Ok(basicInfo);
            }
            catch (Exception)
            {   
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("translated/{pokemonName}")]
        public async Task<IActionResult> GetTranslatedDescription(string pokemonName)
        {
            try
            {
                var res = await GetBasicInfo(pokemonName) as OkObjectResult;
                var basicInfo = (BasicPokemonInformation) res.Value;

                await _pokemonService.TranslateDescription(basicInfo);

                return Ok(basicInfo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
