using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebapiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }
        // GET: api/<CommentController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAsync()
        {
            return (await _commentService.GetCommentsAsync()).ToList();
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<CommentDto>> GetAsync(int id)
        {
            return await _commentService.GetCommentAsync(id);
        }

        // POST api/<CommentController>
        [HttpPost]
        public async Task<ActionResult<CommentDto>> PostAsync([FromForm] CommentCreateDto comment)
        {
            var created = await _commentService.InsertCommentAsync(comment);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id } ,created);
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
