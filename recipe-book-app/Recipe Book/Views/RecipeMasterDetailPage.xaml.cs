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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecipeMasterDetailPage : Page
    {
        private RecipeList recipes;

        public RecipeMasterDetailPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int index = -1;
            if (e.Parameter == null)
            {
                recipes = App.recipes;
                int numRecipes = recipes.getRecipeList().Count;
                if (numRecipes > 0)
                {
                    index = 0;
                }
            }
            else
            {
                recipes = (RecipeList)e.Parameter;
                index = recipes.getSelectedIndex();
            }
            this.recipeListView.ItemsSource = recipes.Recipes;

            if (index < 0)
            {
                detailFrame.Visibility = Visibility.Collapsed;
            }
            recipeListView.SelectedIndex = index;

            updateLayoutFromState(AdaptiveStates.CurrentState, null);
            showDetailView(index);
        }

        private void updateLayout(object sender, VisualStateChangedEventArgs e)
        {
            VisualState newState = e.NewState;
            VisualState oldState = e.OldState;

            updateLayoutFromState(newState, oldState);
        }

        private void updateLayoutFromState(VisualState newState, VisualState oldState)
        {
            bool isNarrow = newState == NarrowState;
            if (isNarrow && oldState == DefaultState)
            {
                // window resized down
                if (!recipes.isEmpty())
                {
                    if (recipes.isEditing())
                    {
                        // TODO: will overwrite changes that are not saved
                        Frame.Navigate(typeof(RecipeForm), recipes, new SuppressNavigationTransitionInfo());
                    } else
                    {
                        Frame.Navigate(typeof(DetailSection), recipes, new SuppressNavigationTransitionInfo());
                    }
                }
            }
        }

        private void showDetailView(int itemIndex)
        {
            if (itemIndex >= 0)
            {
                recipes.SelectedIndex = itemIndex;
                if (isNarrow())
                {
                    Frame.Navigate(typeof(DetailSection), recipes, new DrillInNavigationTransitionInfo());
                }
                else
                {
                    detailFrame.ContentTransitions.Clear();
                    detailFrame.ContentTransitions.Add(new EntranceThemeTransition());
                    if (recipeListView.SelectionMode == ListViewSelectionMode.None)
                    {
                        recipeListView.SelectionMode = ListViewSelectionMode.Single;
                    }
                    detailFrame.Navigate(typeof(DetailSection), recipes);
                }
            }
        }

        private bool isNarrow()
        {
            return Window.Current.Bounds.Width < 720;
        }

        private void showDetailView(object sender, ItemClickEventArgs e)
        {
            Recipe r = (Recipe)e.ClickedItem;
            int itemIndex = 0;
            if (r != null)
            {
                itemIndex = recipes.getRecipeList().IndexOf(r);
            }

            showDetailView(itemIndex);
        }

        private void showSettingsPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        /*
        private void deleteRecipe(object sender, RoutedEventArgs e)
        {
            Recipe recipeToDelete =
                (Recipe)((MenuFlyoutItem)e.OriginalSource).DataContext;
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
                    this.recipeListView.SelectedIndex = currSelectedRecipe - 1;
                }
                else if (currSelectedRecipe == oldIndex && currSelectedRecipe == 0)
                {
                    this.recipeListView.SelectedIndex = 0;
                }
                else
                {
                    this.recipeListView.SelectedIndex = currSelectedRecipe;
                }
                recipes.setSelected(this.recipeListView.SelectedIndex);
                recipes.removeRecipe(recipeToDelete);
            }
        }

        private void editSelectedRecipe(object sender, RoutedEventArgs e)
        {
            recipes.setSelected(this.recipeListView.SelectedIndex);
            recipes.setEditing(true);
            Frame.Navigate((typeof(RecipeForm)), recipes);
        }
        

        private void deleteSelectedRecipe(object sender, RoutedEventArgs e)
        {
            Recipe recipeToDelete = (Recipe)this.recipeListView.SelectedItem;
            tryDeleteRecipe(recipeToDelete);
        }
        */
    }
}
