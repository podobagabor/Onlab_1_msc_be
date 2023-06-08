using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Dtos
{
    public record RecipeDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public double? Rating { get; set; }
        public string Photo { get; set; } = null!;

        public UserDto User { get; set; } = null!;
        public ICollection<DescriptionDto> Descriptions { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<IngredientGroupDto> Ingredients { get; set; }

        public RecipeDto() 
        { 
            Descriptions = new List<DescriptionDto>();
            Comments = new List<CommentDto>();
            Ingredients = new List<IngredientGroupDto>();
        }
    };

    public record RecipeCreateDto
    {
        public string Name { get; set; } = null!;
        public double? Rating { get; set; }
        public IFormFile Photo { get; set; } = null!;

        public int UserId { get; set; }
        public ICollection<DescriptionCreateDto> Descriptions { get; set; }
        public ICollection<IngredientGroupCreateDto> Ingredients { get; set; }

    }

    public class DescriptionCreateDto 
    {
        public string Text { get; set; } = null!;
        public IFormFile? Photo { get; set; }
    }


    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public IFormFile? Photo { get; set; }
    }

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
    }

  

    public record DescriptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string? Photo { get; set; }
    }

}
