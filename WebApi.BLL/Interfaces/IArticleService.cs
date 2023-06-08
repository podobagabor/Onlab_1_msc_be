using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;

namespace WebApi.BLL.Interfaces
{
    public interface IArticleService
    {
        public Task<ArticleDto> GetArticleAsync(int articleId);
        public Task<IEnumerable<ArticleDto>> GetArticlesAsync();
        public Task<ArticleDto> InsertArticleAsync(ArticleCreateDto article);
        public Task<ArticleDto> UpdateArticleAsync(int articleId, ArticleDto article);
        public Task DeleteArticleAsync(int articleId);
    }
}
