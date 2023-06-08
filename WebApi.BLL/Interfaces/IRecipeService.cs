using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;

namespace WebApi.BLL.Interfaces
{
    public interface IRecipeService
    {
        public Task<RecipeDto> GetRecipeAsync(int recipeId);
        public Task<IEnumerable<RecipeDto>> GetRecipesAsync();
        public Task<RecipeDto> InsertRecipeAsync(RecipeCreateDto recipe);
        public Task<RecipeDto> UpdateRecipeAsync(int recipeId,RecipeDto recipe);
        public Task DeleteRecipeAsync(int recipeId);
    }
}
