using Nyam_Nyam.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для ListOfDishesPage.xaml
    /// </summary>
    public partial class ListOfDishesPage : Page
    {
        public static List<Dish> dishes { get; set; }
        public static List<Category> categories { get; set; }
        public ListOfDishesPage()
        {
            InitializeComponent();
            dishes = new List<Dish>(DBConnection.nyamNyam.Dish.ToList());
            categories = new List<Category>(DBConnection.nyamNyam.Category.ToList());
            DishesLv.ItemsSource = dishes;
            categories.Insert(0, new Category() { Name = "All categories" });
            CategoryCB.SelectedIndex = 0;
            double max = dishes[0].OurCost;
            double min = dishes[0].OurCost;


            foreach (Dish dish in dishes)
            {
                if(dish.OurCost > max)
                {
                    max = dish.OurCost;
                }
                if(dish.OurCost < min)
                {
                    min = dish.OurCost;
                }
            }
            PriceSlider.Maximum = max;
            PriceSlider.Minimum = min;
            PriceSlider.Value = max;
            this.DataContext = this;
        }

        private void Refresh()
        {
            var filterDishes = DBConnection.nyamNyam.Dish.ToList();
            var category = CategoryCB.SelectedItem as Category;

            if (category != null && category.Id != 0)
            {
                filterDishes = filterDishes.Where(d => d.CategoryId == category.Id).ToList();
            }
                
            if (NameTB.Text.Length > 0)
            {
                filterDishes = filterDishes.Where(i => i.Name.ToLower().StartsWith(NameTB.Text.Trim().ToLower())
                   || i.Description.ToLower().StartsWith(NameTB.Text.Trim().ToLower())).ToList();
            }

            if((bool)CheckIngrCB.IsChecked)
                filterDishes = filterDishes.Where(i => i.IsAvailable == true).ToList();

            filterDishes = filterDishes.Where(x => (double)x.OurCost <= PriceSlider.Value).ToList();

            DishesLv.ItemsSource = filterDishes;
        }

        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void NameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void CheckIngrCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void DishesLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDish = DishesLv.SelectedItem as Dish;
            if (selectedDish != null)
            {
                NavigationService.Navigate(new CookingPage(selectedDish));
            }
        }

        private void PriceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Refresh();
        }
    }
}
