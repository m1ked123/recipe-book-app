using Recipe_Book.Models;
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
        Recipe recipe;
        public RecipeForm()
        {
            this.InitializeComponent();
            recipe = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("Navigated to new detail page");

            recipe = (Recipe)e.Parameter;

        }

        private void saveRecipe(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Object Name: " + recipe.Name);
            Debug.WriteLine("Control Name: " + this.recipeName.Text);
            Debug.WriteLine("Object Rating: " + recipe.Rating);
            Debug.WriteLine("Conrtol Value: " + this.recipeRating.Value);
        }
    }
}
