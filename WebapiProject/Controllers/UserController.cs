using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BLL.Dtos;
using WebApi.DAL.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebapiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager,SignInManager<User> signInManager, IMapper mapper) {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password,true,false);
            if (!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
            return Ok(result);
        }
        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerValue)
        {
            var user = new User
            {
                UserName = registerValue.Email,
                Email = registerValue.Email,
                Name = registerValue.Name
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, registerValue.Password);
            var result = await _userManager.CreateAsync(user);
            if(!result.Succeeded) 
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var value = await _userManager.GetUserAsync(User);
            var user = new UserDto
            {
                Id = value.Id,
                Email = value.Email,
                Name = value.Name,
                UserName = value.Name,
            };
            
            return Ok(user);
        }
    }
}
