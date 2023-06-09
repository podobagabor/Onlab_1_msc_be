using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;
using WebApi.DAL.Context;
using WebApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly WebApiDbContext _context;
        private readonly IMapper _mapper;

        public ArticleService(WebApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            try
            {
                var existingArticle = await GetArticleAsync(articleId);
                var efArticle = await _context.Articles.FindAsync(articleId);

                foreach (CommentDto comment1 in existingArticle.Comments)
                {
                    var dbComment = await _context.Comments.FindAsync(comment1.Id);
                    if (dbComment != null)
                    {
                        _context.Comments.Remove(dbComment);
                    }
                }
                await _context.SaveChangesAsync();
                _context.Articles.Remove(efArticle);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("elem nem található");
            }
        }

        public async Task<ArticleDto> GetArticleAsync(int articleId)
        {
            return await _context.Articles
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(r => r.Id == articleId)
                ?? throw new Exception("Nem taláható entitás");
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync()
        {
            var articles = await _context.Articles
                .ProjectTo<ArticleDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return articles;
        }

        public async Task<ArticleDto> InsertArticleAsync(ArticleCreateDto article)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=containeraccount;AccountKey=HG14o1kL6C3LSnRbOSREedGlPl/Fd9TNLNGSgldW6Itd6Dqm4I9rEfQtdpsBLqw0AWMbydHH76WM+ASt8WLdXw==;EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("msc-onlab");

            string blobName = Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (Stream stream = article.Photo.OpenReadStream())
            {
                var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };
                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }
            string blobUrl = blobClient.Uri.ToString();
            var efComments = new List<Comment>();
            var efArticle = new Article()
            {
                Title = article.Title,
                Photo = blobUrl,
                UserId = article.UserId,
                Comments = efComments,
                Body = article.Body,
            };
            _context.Articles.Add(efArticle);
            await _context.SaveChangesAsync();
            return await GetArticleAsync(efArticle.Id);
        }

        public async Task<ArticleDto> UpdateArticleAsync(int articleId, ArticleDto article)
        {
            // Retrieve the existing recipe from the database
            var existingArticle = await GetArticleAsync(articleId);
            var efArticle = await _context.Articles.FindAsync(articleId);
            if (existingArticle == null)
            {
                throw new Exception("Recipe not found");
            }

            foreach (CommentDto comment1 in existingArticle.Comments)
            {
                var dbComment = await _context.Comments.FindAsync(comment1.Id);
                if (dbComment != null)
                {
                    _context.Comments.Remove(dbComment);
                }
            }

            // Update the properties of the existing recipe
            efArticle.Title = article.Title;
            efArticle.Photo = article.Photo;

            foreach (CommentDto comment in article.Comments)
            {

                efArticle.Comments.Add(new Comment()
                {
                    Photo = comment.Photo,
                    Rating = comment.Rating,
                    UserId = comment.UserId,
                    Body = comment.Body,
                });
            }

            // Update other properties as needed

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Retrieve the updated recipe
            var updatedArticle = await GetArticleAsync(articleId);
            return updatedArticle;
        }
    }
}
