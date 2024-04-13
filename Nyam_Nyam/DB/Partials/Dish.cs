using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyam_Nyam.DB
{
    public partial class Dish
    {
        public int AllTime
        {
            get
            {
                return (int)this.CookingStage.Sum(s => s.TimeInMinutes);
            }
        }
        public string OpacityDish
        {
            get
            {
                var destinationFormat = string.Empty;
                var allIngredientRecipeSteps = this.CookingStage.SelectMany(x => x.IngredientOfStage);
                if (allIngredientRecipeSteps.Any())
                {
                    foreach (var ingredientStep in allIngredientRecipeSteps)
                    {
                        if (allIngredientRecipeSteps.Where(x => x.IngredientId == ingredientStep.IngredientId).Sum(x => x.Quantity)> ingredientStep.Ingredient.AvailableCount)
                        {
                            destinationFormat = "Gray32Float";
                            IsAvailable = false;
                            DBConnection.nyamNyam.SaveChanges();
                        }
                    }
                }
                else
                {
                    destinationFormat = "Bgra32";
                    IsAvailable = true;
                    DBConnection.nyamNyam.SaveChanges();
                }
                return destinationFormat;
            }
        }

       

        public double OurCost
        {
            get
            {
               
                var v = this.CookingStage.SelectMany(s => s.IngredientOfStage).ToList();
                double result = 0;
                foreach (var i in v)
                {
                    result += i.Ingredient.PriceInDollars * i.Quantity;
                }
                return result;

            }
        }

    }
}
