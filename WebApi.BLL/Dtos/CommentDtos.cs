using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.BLL.Dtos
{
    public record CommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; } = null!;
        public double? Rating { get; set; }
        public string? Photo { get; set; }
        public int UserId { get; set; }
    }

    public record CommentCreateDto
    {
        public string Body { get; set; } = null!;
        public double? Rating { get; set; }
        public IFormFile? Photo { get; set; }
        public int UserId { get; set; }
    }
}
