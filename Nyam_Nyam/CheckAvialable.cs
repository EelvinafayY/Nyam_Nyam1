using Nyam_Nyam.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyam_Nyam
{
    public class CheckAvialable
    {
        public static void Start()
        {
            List<Dish> dishes = DBConnection.nyamNyam.Dish.ToList();
            for (int i = 0; i < dishes.Count; i++)
            {
                dishes[i].IsAvailable = true;
                int id = dishes[i].Id;
                List<CookingStage> cookingStages = DBConnection.nyamNyam.CookingStage.Where(x => x.DishId == id).ToList();
                List<IngredientOfStage> ingredientOfStages = new List<IngredientOfStage>();
                for (int j = 0; j < cookingStages.Count; j++)
                {
                    //int stageld = cookingStages[j].Id;
                    //IngredientOfStage temp = DBConnection.nyamNyam.IngredientOfStage.FirstOrDefault(x => x.CookingStageId == stageld);
                    //if (temp != null)
                    //    ingredientOfStages.Add(temp);

                    //var destinationFormat = string.Empty;
                    //var allIngredientRecipeSteps = this.CookingStage.SelectMany(x => x.IngredientOfStage);
                    //if (allIngredientRecipeSteps.Any())
                    //{
                    //    foreach (var ingredientStep in allIngredientRecipeSteps)
                    //    {
                    //        if (ingredientStep.Quantity * this.BaseServingsQuantity > ingredientStep.Ingredient.AvailableCount)
                    //        {
                    //            destinationFormat = "Gray32Float";
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    destinationFormat = "Bgra32";
                    //}
                }
                for (int j = 0; j < ingredientOfStages.Count; j++)
                {
                    if (ingredientOfStages[j].Ingredient.AvailableCount < ingredientOfStages[j].Quantity)
                        dishes[i].IsAvailable = false;
                }
            }
            DBConnection.nyamNyam.SaveChanges();
        }
    }
}
