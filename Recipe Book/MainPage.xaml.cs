using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Recipe_Book
{
    /// <summary>
    /// This is the main page of the Recipe Book app. It represents
    /// the main page that opens when the app is opened.
    /// </summary>
    // TODO: make this UI responsive
    // TODO: make the detail UI show nothing is the list is empty
    public sealed partial class MainPage : Page
    {
        private RecipeList recipes;
        private Random r;
        public MainPage()
        {
            this.InitializeComponent();
            r = new Random();
            recipes = App.recipes;
            this.recipeListView.ItemsSource = recipes.getRecipeList();
            for (int i = 0; i < 20; i++)
            {
                Recipe sampleRecipe = new Recipe();
                String recipeName = "Recipe " + i;
                int rating = r.Next(5) + 1;
                int num = r.Next(20) + 1;
                for (int j = 0; j < num; j++)
                {
                    double quantity = r.NextDouble() * 100;
                    String UOM = "Cups";
                    String ingredientName = "Flour";
                    RecipeIngredient newIngredient = new RecipeIngredient(quantity, UOM, ingredientName);
                    sampleRecipe.RecipeIngredients.Add(newIngredient);
                }
                sampleRecipe.Name = recipeName;
                sampleRecipe.Rating = rating;
                recipes.addRecipe(sampleRecipe);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (recipes.getRecipeList().Count > 0)
            {
                this.recipeListView.SelectedIndex = 0;
                this.recipes.setSelected(0);
            }
            else
            {
                this.recipeListView.SelectedIndex = recipes.getSelectedIndex();
            }
        }

        private void addNewRecipe(object sender, RoutedEventArgs e)
        {
            Frame.Navigate((typeof(RecipeForm)), recipes);
        }

        private void deleteRecipe(object sender, RoutedEventArgs e)
        {
            recipes.removeRecipe((Recipe)((MenuFlyoutItem)e.OriginalSource).DataContext);
        }

        private void selectRecipe(object sender, SelectionChangedEventArgs e)
        {
            int selectedRecipe = this.recipeListView.SelectedIndex;
            recipes.setSelected(selectedRecipe);
        }

        private void editRecipe(object sender, RoutedEventArgs e)
        {
            // Debug.WriteLine("Editing a recipe");
            Recipe clickedRecipe = (Recipe)((MenuFlyoutItem)e.OriginalSource).DataContext;
            int editingRecipe = recipes.Recipes.IndexOf(clickedRecipe);
            recipes.setSelected(editingRecipe);
            recipes.setEditing(true);
            Frame.Navigate((typeof(RecipeForm)), recipes);
        }

        private void showFlyoutMenu(object sender, RightTappedRoutedEventArgs e)
        {

        }
    }
}
