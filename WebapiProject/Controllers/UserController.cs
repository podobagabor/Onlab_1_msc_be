using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
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
            if(result.Succeeded)
            {
                return Ok(user);
            }
            return Unauthorized();


        }
        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto registerValue)
        {
            string? blobUrl;
            if (registerValue.Photo != null)
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=containeraccount;AccountKey=HG14o1kL6C3LSnRbOSREedGlPl/Fd9TNLNGSgldW6Itd6Dqm4I9rEfQtdpsBLqw0AWMbydHH76WM+ASt8WLdXw==;EndpointSuffix=core.windows.net");
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("msc-onlab");

                string blobName = Guid.NewGuid().ToString();
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (Stream stream = registerValue.Photo.OpenReadStream())
                {
                    var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };
                    await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
                }
                 blobUrl = blobClient.Uri.ToString();
            } else
            {
                blobUrl = null;
            }
            
            var user = new User
            {
                UserName = registerValue.Email,
                Email = registerValue.Email,
                Name = registerValue.Name,
                Photo = blobUrl,
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
            if(value != null)
            {
                var user = new UserDto
                {
                    Id = value.Id,
                    Email = value.Email,
                    Name = value.Name,
                    UserName = value.Name,
                    Photo = value.Photo,
                };
                return Ok(user);
            }

            return null;
        }
        [HttpGet("findUser")]
        public async Task<ActionResult<UserDto>> GetUserFromId(int id)
        {
            try
            {
                var value = await _userManager.FindByIdAsync(id.ToString());
                var user = new UserDto
                {
                    Id = value.Id,
                    Email = value.Email,
                    Name = value.Name,
                    UserName = value.Name,
                    Photo = value.Photo,
                };
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw new Exception("elem nem található");
            }
        }
    }
}
