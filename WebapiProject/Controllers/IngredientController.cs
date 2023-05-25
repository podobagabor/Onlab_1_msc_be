using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;

namespace WebapiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;
        private readonly IMapper _mapper;

        public IngredientController(IIngredientsService ingredientsService, IMapper mapper)
        {
            _ingredientsService = ingredientsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientItemDto>>> GetAsync()
        {
            return (await _ingredientsService.GetIngredientAsync()).ToList();
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<IngredientItemDto>> GetAsync(int id)
        {
            return await _ingredientsService.GetIngredientAsync(id);
        }
        
        // POST api/<RecipeController>
        [HttpPost]
        public async Task<ActionResult<IngredientItemDto>> PostAsync([FromBody] IngredientItemCreateDto ingredient)
        {

            var created = await _ingredientsService.InsertIngredientAsync(ingredient);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }
        /*

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

        */
    }
}
