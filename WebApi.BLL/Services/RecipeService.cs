using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;
using WebApi.DAL.Context;
using WebApi.DAL.Entities;

namespace WebApi.BLL.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly WebApiDbContext _context;
        private readonly IMapper _mapper;

        public RecipeService(WebApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteRecipeAsync(int recipeId)
        {
            _context.Recipes.Remove(new DAL.Entities.Recipe() { Id = recipeId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("elem nem található");
            }
        }

        public async Task<RecipeDto> GetRecipeAsync(int recipeId)
        {
            return await _context.Recipes
                .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(r => r.Id == recipeId)
                ?? throw new Exception("Nem taláható entitás");
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesAsync()
        {
            var recipes = await _context.Recipes
                .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return recipes;
        }

        public async Task<RecipeDto> InsertRecipeAsync(RecipeCreateDto recipe)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=containeraccount;AccountKey=HG14o1kL6C3LSnRbOSREedGlPl/Fd9TNLNGSgldW6Itd6Dqm4I9rEfQtdpsBLqw0AWMbydHH76WM+ASt8WLdXw==;EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("msc-onlab");

            string blobName = Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (Stream stream = recipe.Photo.OpenReadStream())
            {
                var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };
                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }
            string blobUrl = blobClient.Uri.ToString();
            var efComments =  new List<Comment>();
            var efDescriptions = new List<Description>();
            foreach (DescriptionCreateDto descriptionDto in recipe.Descriptions)
            {
                string descriptionBlobName = Guid.NewGuid().ToString();
                BlobClient descriptionBlobClient = containerClient.GetBlobClient(descriptionBlobName);
                using (Stream stream = descriptionDto.Photo.OpenReadStream())
                {
                    var blobHttpHeader = new BlobHttpHeaders { ContentType = "image/jpeg" };
                    await descriptionBlobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
                }
                string descriptionblobUrl = descriptionBlobClient.Uri.ToString();
                efDescriptions.Add(new Description()
                {
                    Text = descriptionDto.Text,
                    Photo = descriptionblobUrl,
                }); ;
            }
            var efIngridientsGroup = new List<IngredientGroup>();
            foreach (IngredientGroupCreateDto ingredienGroupDto in recipe.Ingredients)
            {
                var efIngridientItems = new List<IngredientItem>();
                foreach (IngredientCreateDto ingredientItemDto in ingredienGroupDto.Ingredients)
                {
                    efIngridientItems.Add(new IngredientItem()
                    {
                        Amount = ingredientItemDto.Amount,
                        IngredientId = ingredientItemDto.Id
                    });
                }
                efIngridientsGroup.Add(new IngredientGroup()
                {
                    Name = ingredienGroupDto.Name,
                    Ingredients = efIngridientItems,
                });
            }
            var efRecipe = new Recipe()
            {
                Name = recipe.Name,
                Photo = blobUrl,
                UserId = recipe.UserId,
                Comments = efComments,
                Descriptions = efDescriptions,
                Ingredients = efIngridientsGroup,

            };
            _context.Recipes.Add(efRecipe);
            await _context.SaveChangesAsync(); 
            return await GetRecipeAsync(efRecipe.Id);
        }

        public async Task UpdateRecipeAsync(int recipeId,RecipeDto recipe)
        {
            var efRecipe = _mapper.Map<DAL.Entities.Recipe>(recipe);
            efRecipe.Id = recipeId;
            var entry = _context.Attach(efRecipe);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Nem található entitás");
            }
        }

        private async Task<byte[]> GetByteArrayFromImageAsync(byte[] image)
        {
            using var ms = new MemoryStream();
           // await image.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
