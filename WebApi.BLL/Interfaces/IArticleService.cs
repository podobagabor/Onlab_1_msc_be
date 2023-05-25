using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Interfaces
{
    public interface IArticleService
    {
        public Task<Article> GetArticleAsync(int articleId);
        public Task<IEnumerable<Article>> GetArticleAsync();
        public Task<Article> InsertArticleAsync(Article article);
        public Task<Article> UpdateArticleAsync(Article article);
        public Task<Article> DeleteArticleAsync(int articleId);
    }
}
