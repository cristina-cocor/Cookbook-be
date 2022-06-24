using CookbookBE.Controllers.Book;
using CookbookBE.DataLayer;
using CookbookBE.DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CookbookController : ControllerBase
    {
        private readonly ApplicationContext _db1;

        public CookbookController(ApplicationContext db1)
        {
            _db1 = db1;
        }

        [HttpGet("GetRecipe/{id:int}")]
        
        public ActionResult<Cookbook> GetRecipe(String nume)
        {
            return _db1.Cookbook.Where(c => nume == c.RecipeName).Single();
        }

        [HttpGet("AllRecipes")]

        public ActionResult<GetAllRecipesResponse> GetAllRecipes(int pageSize, int pageNumber, Enums.SortType sortType)
        {
            var allRecipesQuery = _db1.Cookbook.AsQueryable();

            switch (sortType)
            {
                case Enums.SortType.FirstNameAscending:
                    allRecipesQuery = allRecipesQuery.OrderBy(x => x.RecipeName);
                    break;
                case Enums.SortType.FirstNameDescending:
                    allRecipesQuery = allRecipesQuery.OrderByDescending(x => x.RecipeName);
                    break;
                default: throw new ApplicationException("Unknown sort type");
            }

            var allRecipes = allRecipesQuery
                .Select(r => new RecipeDTO
                {                                                                                            
                    Name = r.RecipeName,
                    Ingredients = r.Ingredients,
                    Instructions = r.Instructions,
                    TotalTime = r.TotalTime,
                })
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return Ok(new GetAllRecipesResponse
            {
                Recipes = allRecipes
            });
        }

        [HttpDelete("DeleteRecipe/{id:int}")]

        public ActionResult<bool> Delete(int id)
        {
            try
            {
                var recipeToDelete = _db1.Cookbook.Single(recipe => id == recipe.IdE);

                _db1.Cookbook.Remove(recipeToDelete);
                _db1.SaveChanges();
                return Ok(new { status = true });
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

       

        
    }
}
