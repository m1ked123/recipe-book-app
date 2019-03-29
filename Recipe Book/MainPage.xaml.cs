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
    /// This is the main page of the Recipe Book app. It represents
    /// the main page that opens when the app is opened.
    /// </summary>
    // TODO: make this UI responsive
    // TODO: make the detail UI show nothing is the list is empty
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Recipe> recipes;
        private Recipe currentRecipe;
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
        }

        private void deleteRecipe(object sender, RoutedEventArgs e)
        {
            this.recipes.Remove(((Recipe)((MenuFlyoutItem)e.OriginalSource).DataContext));
        }

        private void selectRecipe(object sender, SelectionChangedEventArgs e)
        {
            Recipe clickedRecipe = (Recipe)this.recipeListView.SelectedItem;
            currentRecipe = clickedRecipe;
        }
    }
}
