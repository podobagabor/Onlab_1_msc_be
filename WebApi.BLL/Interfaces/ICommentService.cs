using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;

namespace WebApi.BLL.Interfaces
{
    public interface ICommentService
    {
        public Task<CommentDto> GetCommentAsync(int comment);
        public Task<IEnumerable<CommentDto>> GetCommentsAsync();
        public Task<CommentDto> InsertCommentAsync(CommentCreateDto comment);
        public Task<CommentDto> UpdateCommentAsync(CommentCreateDto comment);
        public Task DeleteCommentAsync(int comment);
    }
}
