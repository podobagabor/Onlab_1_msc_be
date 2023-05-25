using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Dtos;
using WebApi.BLL.Interfaces;
using WebApi.DAL.Context;
using WebApi.DAL.Entities;
using AutoMapper.QueryableExtensions;

namespace WebApi.BLL.Services
{
    public class IngredientService : IIngredientsService
    {
        private readonly WebApiDbContext _context;
        private readonly IMapper _mapper;

        public IngredientService(WebApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<IngredientItemDto> DeleteIngredientAsync(int ingredient)
        {
            throw new NotImplementedException();
        }

        public async Task<IngredientItemDto> GetIngredientAsync(int ingredient)
        {
            return await _context.Ingredients
                .ProjectTo<IngredientItemDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(r => r.Id == ingredient)
                ?? throw new Exception("Nem található entitás");
        }

        public async Task<IEnumerable<IngredientItemDto>> GetIngredientAsync()
        {
            var ingredients = await _context.Ingredients
                .ProjectTo<IngredientItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return ingredients;
        }

        public async Task<IngredientItemDto> InsertIngredientAsync(IngredientItemCreateDto ingredient)
        {
            var efIngredient = new Ingredient
            {
                Name = ingredient.Name,
                Unit = ingredient.Unit,
            };
            _context.Ingredients.Add(efIngredient);
            await _context.SaveChangesAsync();
            return await GetIngredientAsync(efIngredient.Id);
        }

        public Task<IngredientItemDto> UpdateIngredientAsync(IngredientItemCreateDto ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
