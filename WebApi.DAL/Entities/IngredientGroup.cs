using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Entities
{
    public class IngredientGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;
        public ICollection<IngredientItem> Ingredients { get; set; }
        public IngredientGroup() 
        { 
            Ingredients = new List<IngredientItem>();
        }
    }
}
