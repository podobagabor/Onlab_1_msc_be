using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;
using WebApi.DAL.Context;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly WebApiDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(WebApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                throw new Exception("comment not found");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<CommentDto> GetCommentAsync(int comment)
        {
            return await _context.Comments
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(c => c.Id == comment)
                ?? throw new Exception("Nem található entitás");
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync()
        {
            var comments = await _context.Comments
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return comments;
        }

        public async Task<CommentDto> InsertCommentAsync(CommentCreateDto comment)
        {
            string? blobUrl;
            if(comment.Photo != null)
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=containeraccount;AccountKey=HG14o1kL6C3LSnRbOSREedGlPl/Fd9TNLNGSgldW6Itd6Dqm4I9rEfQtdpsBLqw0AWMbydHH76WM+ASt8WLdXw==;EndpointSuffix=core.windows.net");
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("msc-onlab");
                string blobName = Guid.NewGuid().ToString();
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (Stream stream = comment.Photo.OpenReadStream())
                {
                    var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };
                    await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
                }
                blobUrl = blobClient.Uri.ToString();
            } else { blobUrl = null; }

            var efComment = new Comment()
            {
                Body = comment.Body,
                Photo = blobUrl,
                Rating = comment.Rating,
                UserId = comment.UserId,
            };
            _context.Comments.Add(efComment);
            await _context.SaveChangesAsync();
            return await GetCommentAsync(efComment.Id);
        }

        public Task<CommentDto> UpdateCommentAsync(CommentCreateDto comment)
        {
            throw new NotImplementedException();
        }
    }
}
