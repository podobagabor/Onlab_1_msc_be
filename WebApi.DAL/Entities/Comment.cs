using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;
        public double? Rating { get; set; }
        public string? Photo { get; set; }
        public int UserId { get; set; }
    }
}
