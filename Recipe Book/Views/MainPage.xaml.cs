using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using Recipe_Book.Views;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Recipe_Book
{
    /// <summary>
    /// This is the main page of the Recipe Book app. It represents
    /// the main page that opens when the app is opened.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private RecipeList recipes;
        public MainPage()
        {
            this.InitializeComponent();
            recipes = App.recipes;
            this.recipeListView.ItemsSource = recipes.Recipes;
            Debug.WriteLine("Create the main page");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int index = -1;
            if (e.Parameter == null)
            {
                int numRecipes = recipes.getRecipeList().Count;
                if (numRecipes > 0)
                {
                    index = 0;
                    Debug.WriteLine("Index set to 0 because of recipe number: " + numRecipes);
                }
            }
            else
            {
                index = (int)e.Parameter;
                Debug.WriteLine("Index set from parameter: " + index);
            }

            
            if (index < 0)
            {
                this.detailView.Visibility = Visibility.Collapsed;
            } else
            {
                this.recipeListView.SelectedIndex = index;
                this.recipes.setSelected(index);
                this.detailView.Visibility = Visibility.Visible;
                detailView.SelectedItem = detailView.MenuItems[0];
                contentFrame.Navigate(typeof(DetailPage), recipes);
            }

            updateLayoutFromState(AdaptiveStates.CurrentState, null);
        }

        private void addNewRecipe(object sender, RoutedEventArgs e)
        {
            Frame.Navigate((typeof(RecipeForm)), recipes);
        }

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

        private void updateLayout(object sender, VisualStateChangedEventArgs e)
        {
            VisualState newState = e.NewState;
            VisualState oldState = e.OldState;

            updateLayoutFromState(newState, oldState);
        }

        private void updateLayoutFromState(VisualState newState, VisualState oldState)
        {
            int selectedIndex = recipes.getSelectedIndex();
            Recipe selectedRecipe = null;
            if (selectedIndex >= 0)
            {
                selectedRecipe = recipes.getSelected();
            }
            bool isNarrow = newState == NarrowState;
            if (isNarrow && oldState == DefaultState && selectedRecipe != null)
            {
                // Resize down to the detail item. Don't play a transition.
                Frame.Navigate(typeof(DetailPage), recipes, new SuppressNavigationTransitionInfo());
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(recipeListView, isNarrow);
            if (detailView != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(detailView, !isNarrow);
            }
        }

        private void showDetailView(object sender, ItemClickEventArgs e)
        {
            bool isNarrow = AdaptiveStates.CurrentState == NarrowState;
            int itemIndex = recipes.getRecipeList().IndexOf((Recipe)e.ClickedItem);
            recipes.setSelected(itemIndex);
            if (isNarrow)
            {
                Frame.Navigate(typeof(DetailPage), recipes, new DrillInNavigationTransitionInfo());
            }
            else
            {
                detailView.ContentTransitions.Clear();
                detailView.ContentTransitions.Add(new EntranceThemeTransition());
                detailView.SelectedItem = detailView.MenuItems[0];
                contentFrame.Navigate(typeof(DetailPage), recipes);
            }
        }

        private void showSettingsPage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private void navigateToPage(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            String itemName = args.InvokedItemContainer.Name;
            var preNavPageType = contentFrame.CurrentSourcePageType;

            if (itemName == "recipeContentView") {
                contentFrame.Navigate(typeof(DetailPage), recipes);
            } else
            {
                contentFrame.Navigate(typeof(JournalPage));
            }
        }
    }
}
