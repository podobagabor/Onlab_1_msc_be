using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;

namespace WebapiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }


        // GET: api/<ArticleController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetAsync()
        {
            return (await _articleService.GetArticlesAsync()).ToList();
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<ArticleDto>> GetAsync(int id)
        {
            return await _articleService.GetArticleAsync(id);
        }

        // POST api/<ArticleController>
        [HttpPost]
        public async Task<ActionResult<ArticleDto>> PostAsync([FromForm] ArticleCreateDto article)
        {

            var created = await _articleService.InsertArticleAsync(article);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ArticleDto>> PutAsync(int id, [FromBody] ArticleDto value)
        {
            var updated = await _articleService.UpdateArticleAsync(id, value);
            return Ok(updated);

        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}
