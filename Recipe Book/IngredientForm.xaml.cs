using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
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
    public sealed partial class IngredientForm : Page
    {
        Recipe recipe;
        public IngredientForm()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RecipeList recipes = (RecipeList)e.Parameter;
            recipe = recipes.EditingRecipe;
        }

        private void addIngredient(object sender, RoutedEventArgs e)
        {
            String name = this.ingredientName.Text;
            String unitOfMeaure = this.ingredientUOM.Text;
            double quantity = Double.Parse(this.ingredientQuantity.Text);
            RecipeIngredient newIngredient = new RecipeIngredient(quantity, unitOfMeaure, name);
            recipe.RecipeIngredients.Add(newIngredient);
            Frame.GoBack();
        }

        private void cancel(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
