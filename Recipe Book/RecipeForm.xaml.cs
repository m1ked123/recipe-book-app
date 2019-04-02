using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecipeForm : Page
    {
        RecipeList recipes;
        Recipe recipe;

        public RecipeForm()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("Navigated to new detail page");

            recipes = (RecipeList)e.Parameter;
            if (recipes.isEditing())
            {
                recipe = recipes.getSelected();
            } else
            {
                // we're creating a new recipe
                recipe = new Recipe();
            }

        }

        private void saveRecipe(object sender, RoutedEventArgs e)
        {
            String newRecipeName = this.recipeName.Text;
            double newRecipeRating = this.recipeRating.Value;

            // commit changes to object

            recipe.Name = newRecipeName;
            recipe.Rating = newRecipeRating;

            if (!recipes.isEditing())
            {
                recipes.addRecipe(recipe);
                Debug.WriteLine(recipes.getRecipeList().Count);
            }
            Frame.GoBack();
        }

        private void cancelRecipeCreation(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
