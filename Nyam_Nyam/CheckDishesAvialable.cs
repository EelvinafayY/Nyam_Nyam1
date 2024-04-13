using Nyam_Nyam.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nyam_Nyam.Services
{
    public class CheckDishesAvialable
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
                    int stageld = cookingStages[j].Id;
                    IngredientOfStage temp = DBConnection.nyamNyam.IngredientOfStage.FirstOrDefault(x => x.CookingStageId = stageld);
                    if (temp != null)
                        ingredientOfStages.Add(temp);
                }
                for (int j = 0; j < ingredientOfStages.Count; j++)
                {
                    if (ingredientOfStages[j].Ingredient.AvailableCount < ingredientOfStages[j].Quantity)
                        dishes[i].IsAvailable = false;
                }
            }
            DBConnection.nyamNyam.Dish.SaveChanges();
        }

    }
}
