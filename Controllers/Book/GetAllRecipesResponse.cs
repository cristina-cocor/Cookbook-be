using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookBE.Controllers.Book
{
    public class GetAllRecipesResponse
    {
        public List<RecipeDTO> Recipes { get; set; }

    }

    public class RecipeDTO
    {
        public String Name { get; set; }

        public String Ingredients { get; set; }

        public String Instructions { get; set; }

        public String TotalTime { get; set; }
    }
}
