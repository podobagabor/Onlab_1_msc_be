using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApi.DAL.Entities
{
    public class Description
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string? Photo { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;
    }
}
