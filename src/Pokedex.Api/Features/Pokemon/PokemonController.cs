using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pokedex.Api.Features.Pokemon
{
    /// <summary>
    ///     Pokemon data 
    /// </summary>
    [ApiController]
    [Route("~/api/v1/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        /// <summary>
        ///     Constructs a new instance of PokemonController
        /// </summary>
        /// <param name="pokemonService"></param>
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService ?? throw new ArgumentNullException(nameof(pokemonService));
        }

        /// <summary>
        ///     Fetches a Pokemon's basic information via name
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        [HttpGet("{pokemonName}")]
        public async Task<IActionResult> GetBasicInfo([FromRoute] string pokemonName)
        {   
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return BadRequest("Pokemon name is required.");
            }

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

        /// <summary>
        ///     Fetches a Pokemon's basic information via name with a fun translation applied to the description
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <returns></returns>
        [HttpGet("translated/{pokemonName}")]
        public async Task<IActionResult> GetBasicInfoWithTranslatedDesc([FromRoute] string pokemonName)
        {
            try
            {
                var basicInfoRes = await GetBasicInfo(pokemonName);

                if (basicInfoRes is OkObjectResult)
                {
                    var data = basicInfoRes as OkObjectResult;
                    var basicInfo = (BasicPokemonInformation)data.Value;

                    await _pokemonService.TranslateDescription(basicInfo);

                    return Ok(basicInfo);
                }

                return basicInfoRes switch
                {
                    BadRequestObjectResult => basicInfoRes,
                    NotFoundResult => basicInfoRes,
                    StatusCodeResult => basicInfoRes
                };
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
