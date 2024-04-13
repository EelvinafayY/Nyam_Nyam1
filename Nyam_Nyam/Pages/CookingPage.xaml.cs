using Nyam_Nyam.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nyam_Nyam.Pages
{
    /// <summary>
    /// Логика взаимодействия для CookingPage.xaml
    /// </summary>
    public partial class CookingPage : Page
    {
        Dish contextDish;
        int counter = 1;
        public CookingPage(Dish dish)
        {
            InitializeComponent();
            contextDish = dish;
            Refresh();
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListOfDishesPage());
        }

        private void BMinus_Click(object sender, RoutedEventArgs e)
        {
            if (contextDish.BaseServingsQuantity == 0)
                return;
            if(counter > 1)
            {
                counter--;
            }          
            Refresh();
            DataContext = null;
            DataContext = contextDish;
        }
        private void Refresh()
        {

            DataContext = null;
            CookingStage.stepNumber = 1;
            DataContext = contextDish;
            DescriptionTB.Text = contextDish.Description;
            var t = contextDish.CookingStage.SelectMany(s => s.IngredientOfStage).ToList();
            var v = new List<IngredientOfStage>();
            foreach (var i in t)
            {
                var w = v.Find(x => x.IngredientId == i.IngredientId);
                if(w == null)
                {
                    i.SumQuantity = i.Quantity;
                    v.Add(i);
                }
                else
                {
                    w.SumQuantity += i.Quantity;
                }
                
            }
            for (int i = 0; i < v.Count; i++)
            {
                v[i].TotalQuantity = v[i].SumQuantity * counter;
                v[i].TotalCost = v[i].Ingredient.PriceInDollars * v[i].TotalQuantity;
            }
            TBTotalCost.Text = $"Total cost: {contextDish.OurCost * counter}$";
            DGIngredient.ItemsSource = v;
            TBCount.Text = (contextDish.BaseServingsQuantity * counter).ToString();
            LVRecipesStep.ItemsSource = contextDish.CookingStage.ToList();
        }
        private void BPlus_Click(object sender, RoutedEventArgs e)
        {
            if (contextDish.BaseServingsQuantity == 99)
                return;
            counter++;
            Refresh();
        }

        

        private void TBCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void TBCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            DBConnection.nyamNyam.SaveChanges();
        }

        private void DGIngredient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DBConnection.nyamNyam.SaveChanges();
            Refresh();
        }

      
    }
}
