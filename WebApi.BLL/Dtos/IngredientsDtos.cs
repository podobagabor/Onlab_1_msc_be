using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.BLL.Dtos
{
    public record IngredientGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<IngredientDto> Ingredients { get; set; }

        public IngredientGroupDto()
        {
            Ingredients = Ingredients = new List<IngredientDto>();
        }
    }
    public record IngredientGroupCreateDto
    {
        public string Name { get; set; } = null!;
        public ICollection<IngredientCreateDto> Ingredients { get; set; }

        public IngredientGroupCreateDto()
        {
            Ingredients = Ingredients = new List<IngredientCreateDto>();
        }
    }

    public record IngredientDto
    {
        public int Id { get; init; }
        public int Amount { get; set; } = 0!;
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
    }

    public record IngredientCreateDto
    {
        public int Id { get; init; }
        public int Amount { get; set; } = 0!;
    }

    public record IngredientItemCreateDto
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
    }

    public record IngredientItemDto
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
    }
}
