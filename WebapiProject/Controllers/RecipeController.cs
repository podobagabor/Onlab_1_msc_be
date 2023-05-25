using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;
using WebApi.DAL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebapiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeService recipeService, IMapper mapper)
        {
            _recipeService = recipeService;
            _mapper = mapper;
        }


        // GET: api/<RecipeController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAsync()
        {
            return (await _recipeService.GetRecipesAsync()).ToList();
        }

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<RecipeDto>> GetAsync(int id)
        {
            return await _recipeService.GetRecipeAsync(id);
        }

        // POST api/<RecipeController>
        [HttpPost]
        public async Task<ActionResult<RecipeDto>> PostAsync([FromForm] RecipeCreateDto recipe)
        {
            
            var created = await _recipeService.InsertRecipeAsync(recipe);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }

        // PUT api/<RecipeController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] RecipeDto value)
        {
            await _recipeService.UpdateRecipeAsync(id, value);
            return NoContent();
        }

        // DELETE api/<RecipeController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }
    }
}
