using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double? Rating { get; set; }
        public string Photo { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Description> Descriptions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<IngredientGroup> Ingredients { get; set; }
        public Recipe( )
        {
            Comments = new List<Comment>();
            Descriptions = new List<Description>();
            Ingredients = new List<IngredientGroup>();

        }
    }
}
