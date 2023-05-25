using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Dtos
{
    public class WebApiProfile : Profile
    {
        public WebApiProfile() 
        {
            CreateMap<DAL.Entities.Recipe, RecipeDto>().ReverseMap();
            CreateMap<DAL.Entities.IngredientItem, IngredientDto>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(ingredient => ingredient.Ingredient.Id)).ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(Ingredient => Ingredient.Ingredient.Name)).ForMember(
                dest => dest.Unit,
                opt => opt.MapFrom(Ingredient => Ingredient.Ingredient.Unit))
                .ReverseMap();
            CreateMap<DAL.Entities.Ingredient, IngredientItemDto>().ReverseMap();
            CreateMap<DAL.Entities.IngredientGroup,IngredientGroupDto>().ReverseMap();
            CreateMap<DAL.Entities.Description, DescriptionDto>().ReverseMap();
            CreateMap<DAL.Entities.Comment, CommentDto>().ReverseMap();
            CreateMap<DAL.Entities.User, UserDto>().ReverseMap();
        }
    }
}
