using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public UserDto User { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public ICollection<CommentDto> Comments { get; set; }

        public ArticleDto()
        {
            Comments = new List<CommentDto>();
        }
    }

    public class ArticleCreateDto
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public int UserId { get; set; }
        public IFormFile Photo { get; set; } = null!;

    }
}
