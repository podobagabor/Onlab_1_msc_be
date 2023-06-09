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
            try
            {
                var existingRecipe = await GetRecipeAsync(recipeId);
                foreach (CommentDto comment1 in existingRecipe.Comments)
                {
                    var dbComment = await _context.Comments.FindAsync(comment1.Id);
                    if (dbComment != null)
                    {
                        _context.Comments.Remove(dbComment);
                    }
                }

                foreach (DescriptionDto description1 in existingRecipe.Descriptions)
                {
                    var dbDescription = await _context.Descriptions.FindAsync(description1.Id);
                    if (dbDescription != null)
                    {
                        _context.Descriptions.Remove(dbDescription);
                    }
                }

                foreach (IngredientGroupDto ingredientGroupDto in existingRecipe.Ingredients)
                {
                    foreach (IngredientDto ingredientDto in ingredientGroupDto.Ingredients)
                    {
                        var dbIngredientItemDto = await _context.IngredientItems.FindAsync(ingredientDto.Id);
                        if (dbIngredientItemDto != null)
                        {
                            _context.IngredientItems.Remove(dbIngredientItemDto);
                        }
                    }
                    var dbIngredientGroupDto = await _context.IngredientGroups.FindAsync(ingredientGroupDto.Id);
                    if (dbIngredientGroupDto != null)
                    {
                        _context.IngredientGroups.Remove(dbIngredientGroupDto);
                    }
                }
                await _context.SaveChangesAsync();
                var entity = await _context.Recipes.FindAsync(recipeId);
                _context.Recipes.Remove(entity);
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
            var efComments = new List<Comment>();
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

        public async Task<RecipeDto> UpdateRecipeAsync(int recipeId, RecipeDto recipe)
        {
            // Retrieve the existing recipe from the database
            var existingRecipe = await GetRecipeAsync(recipeId);
            var efRecipe = await _context.Recipes.FindAsync(recipeId);
            if (existingRecipe == null)
            {
                throw new Exception("Recipe not found");
            }

            foreach(CommentDto comment1 in existingRecipe.Comments)
            {
                var dbComment = await _context.Comments.FindAsync(comment1.Id);
                if (dbComment != null)
                {
                    _context.Comments.Remove(dbComment);
                }
            }

            foreach(DescriptionDto description1 in existingRecipe.Descriptions)
            {
                var dbDescription = await _context.Descriptions.FindAsync(description1.Id);
                if (dbDescription != null)
                {
                    _context.Descriptions.Remove(dbDescription);
                }
            }

            foreach(IngredientGroupDto ingredientGroupDto in existingRecipe.Ingredients)
            {
                foreach(IngredientDto ingredientDto in ingredientGroupDto.Ingredients)
                {
                    var dbIngredientItemDto = await _context.IngredientItems.FindAsync(ingredientDto.Id);
                    if(dbIngredientItemDto != null)
                    {
                        _context.IngredientItems.Remove(dbIngredientItemDto);
                    }
                }
                var dbIngredientGroupDto = await _context.IngredientGroups.FindAsync(ingredientGroupDto.Id);
                if (dbIngredientGroupDto != null)
                {
                    _context.IngredientGroups.Remove(dbIngredientGroupDto);
                }
            }

            // Update the properties of the existing recipe
            efRecipe.Name = recipe.Name;
            efRecipe.Photo = recipe.Photo;
            efRecipe.Rating = recipe.Rating;

            foreach (DescriptionDto description in recipe.Descriptions)
            {
                efRecipe.Descriptions.Add(new Description()
                {
                    Text = description.Text,
                    Photo = description.Photo,
                });
            }

            foreach (CommentDto comment in recipe.Comments)
            {

                efRecipe.Comments.Add(new Comment()
                {
                    Photo = comment.Photo,
                    Rating = comment.Rating,
                    UserId = comment.UserId,
                    Body = comment.Body,
                });
            }
          
             foreach (IngredientGroupDto ingredienGroupDto in recipe.Ingredients)
            {
                var newGroup = new IngredientGroup()
                {
                    Name = ingredienGroupDto.Name,
                };
                foreach (IngredientDto ingredientItemDto in ingredienGroupDto.Ingredients)
                {
                    newGroup.Ingredients.Add(new IngredientItem()
                    {
                        Amount = ingredientItemDto.Amount,
                        IngredientId = ingredientItemDto.Id
                    });
                }
                efRecipe.Ingredients.Add(newGroup);
            }
            

            // Update other properties as needed

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Retrieve the updated recipe
            var updatedRecipe = await GetRecipeAsync(recipeId);
            return updatedRecipe;
        }

    }
}
