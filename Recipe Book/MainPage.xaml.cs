using Recipe_Book.Models;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Recipe_Book
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Recipe> recipes;
        public MainPage()
        {
            this.InitializeComponent();
            recipes = new ObservableCollection<Recipe>();
            this.recipeListView.ItemsSource = recipes;
        }

        private void addNewRecipe(object sender, RoutedEventArgs e)
        {
            // TODO: this does not 
            Recipe newRecipe = new Recipe();
            this.recipes.Add(newRecipe);
            Debug.WriteLine("Navigating to new form page...");
            recipeDetailFrame.Navigate((typeof(RecipeForm)), newRecipe);
        }

        private void showRecipe(object sender, SelectionChangedEventArgs e)
        {
            Recipe selectedRecipe = (Recipe)e.AddedItems[0];
            Debug.WriteLine("Navigating to new detail page...");
            recipeDetailFrame.Navigate((typeof(RecipeDetailPage)), selectedRecipe);
        }
    }
}
