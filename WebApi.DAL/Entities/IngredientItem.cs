using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Entities
{
    public class IngredientItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int IngredientId { get; set; }
        public int IngredientGroupId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
        public IngredientGroup IngredientGroup { get; set;} = null!;
        
    }
}
