using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using Recipe_Book.Views;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Recipe_Book
{
    /// <summary>
    /// This is the main page of the Recipe Book app. It represents
    /// the main page that opens when the app is opened.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        private RecipeList currentList;
        public AppShell()
        {
            this.InitializeComponent();
            currentList = App.recipes;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mainContent.Navigate(typeof(RecipeMasterDetailPage), currentList);
        }

        private void navigateView(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                mainContent.Navigate(typeof(SettingsPage));
            } else if (args.InvokedItemContainer == MainRecipeBook)
            {
                mainContent.Navigate(typeof(RecipeMasterDetailPage), currentList);
            } else if (args.InvokedItemContainer == NewRecipeButton)
            {
                mainContent.Navigate(typeof(RecipeForm), currentList);
            }
        }
    }
}
