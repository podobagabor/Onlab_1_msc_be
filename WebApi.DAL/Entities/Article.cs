using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Entities
{
    public class Article
    { 
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; }
        public Article()
        {
            Comments = new List<Comment>();
        }
    }
}
