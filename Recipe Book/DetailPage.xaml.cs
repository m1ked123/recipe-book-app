using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private Recipe recipe;
        private RecipeList recipes;
        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recipes = (RecipeList)e.Parameter;
            recipe = recipes.getSelected();

            IList<PageStackEntry> backStack = Frame.BackStack;
            int backStackCount = backStack.Count;
            if (backStackCount > 0)
            {
                PageStackEntry masterPageEntry = backStack[backStackCount - 1];
                backStack.RemoveAt(backStackCount - 1);
                
                PageStackEntry modifiedEntry = new PageStackEntry(
                    masterPageEntry.SourcePageType,
                    recipes.getSelectedIndex(),
                    masterPageEntry.NavigationTransitionInfo
                    );
                backStack.Add(modifiedEntry);
            }

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += onBackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= onBackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void onBackRequested(object sender, BackRequestedEventArgs e)
        {
            // Page above us will be our master view.
            // Make sure we are using the "drill out" animation in this transition.
            e.Handled = true;
            Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private void editSelectedRecipe(object sender, RoutedEventArgs e)
        {
            recipes.setEditing(true);
            Frame.Navigate((typeof(RecipeForm)), recipes);
        }

        private void deleteSelectedRecipe(object sender, RoutedEventArgs e)
        {
            Recipe recipeToDelete = recipes.getSelected();
            tryDeleteRecipe(recipeToDelete);
        }

        private async void tryDeleteRecipe(Recipe recipeToDelete)
        {
            ContentDialog deleteConfirmationDialog = new ContentDialog
            {
                Title = "Permenantly delete recipe?",
                Content = "If you delete this recipe, you won't be" +
                " able to recover it. Are you Sure you want to delete" +
                " it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                int oldIndex = recipes.getRecipeList().IndexOf(recipeToDelete);

                int currSelectedRecipe = recipes.getSelectedIndex(); ;

                if (currSelectedRecipe == oldIndex && currSelectedRecipe > 0)
                {
                    recipes.setSelected(currSelectedRecipe - 1);
                }
                else if (currSelectedRecipe == oldIndex && currSelectedRecipe == 0)
                {
                    recipes.setSelected(0);
                }
                else
                {
                    recipes.setSelected(currSelectedRecipe);
                }
                recipes.removeRecipe(recipeToDelete);

                IList<PageStackEntry> backStack = Frame.BackStack;
                int backStackCount = backStack.Count;
                if (backStackCount > 0)
                {
                    PageStackEntry masterPageEntry = backStack[backStackCount - 1];
                    backStack.RemoveAt(backStackCount - 1);

                    PageStackEntry modifiedEntry = new PageStackEntry(
                        masterPageEntry.SourcePageType,
                        recipes.getSelectedIndex(),
                        masterPageEntry.NavigationTransitionInfo
                        );

                    backStack.Add(modifiedEntry);
                }
                Frame.GoBack();
            }
        }
    }
}
