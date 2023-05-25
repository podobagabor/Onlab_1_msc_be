using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Interfaces
{
    public interface IIngredientsService
    {
        public Task<IngredientItemDto> GetIngredientAsync(int ingredient);
        public Task<IEnumerable<IngredientItemDto>> GetIngredientAsync();
        public Task<IngredientItemDto> InsertIngredientAsync(IngredientItemCreateDto ingredient);
        public Task<IngredientItemDto> UpdateIngredientAsync(IngredientItemCreateDto ingredient);
        public Task<IngredientItemDto> DeleteIngredientAsync(int ingredient);
    }
}
