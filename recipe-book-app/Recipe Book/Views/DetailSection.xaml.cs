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

namespace Recipe_Book.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailSection : Page
    {
        private RecipeList recipes;
        private Recipe selectedRecipe;

        public DetailSection()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recipes = (RecipeList)e.Parameter;
            selectedRecipe = recipes.getSelected();
            detailSection.SelectedItem = detailSection.MenuItems[0];
            contentFrame.Navigate(typeof(DetailPage), selectedRecipe);
            if (isNarrow())
            {
                IList<PageStackEntry> backStack = Frame.BackStack;
                int backStackCount = backStack.Count;
                if (backStackCount > 0)
                {
                    PageStackEntry masterPageEntry = backStack[backStackCount - 1];
                    backStack.RemoveAt(backStackCount - 1);

                    PageStackEntry modifiedEntry = new PageStackEntry(
                        masterPageEntry.SourcePageType,
                        recipes,
                        masterPageEntry.NavigationTransitionInfo
                        );

                    backStack.Add(modifiedEntry);
                }

                // Show back button
                NavigationView currShell = AppShell.currentShell;
                currShell.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
                currShell.IsBackEnabled = true;
            }
        }

        private void editSelectedRecipe(object sender, RoutedEventArgs e)
        {
            recipes.setEditing(true);
            Frame.Navigate((typeof(RecipeForm)), recipes);
        }

        private void deleteSelectedRecipe(object sender, RoutedEventArgs e)
        {
            tryDeleteRecipe(selectedRecipe);
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
                int deleteIndex = recipes.getRecipeList().IndexOf(recipeToDelete);

                if (deleteIndex > 0)
                {
                    // deleting recipe somewhere other than beginning
                    // select the recipe before it
                    recipes.SelectedIndex = deleteIndex - 1;
                }
                else if (deleteIndex == 0)
                {
                    // deleting recipe at the beginning
                    // select the same index
                    recipes.SelectedIndex = 0;
                }

                recipes.removeRecipe(recipeToDelete);
                if (isNarrow())
                {
                    Frame.GoBack();
                }
                else
                {
                    if (recipes.Recipes.Count == 0)
                    {
                        // hide this column
                        detailSectionGrid.Visibility = Visibility.Collapsed;
                    }
                    selectedRecipe = recipes.getSelected();
                    detailSection.SelectedItem = detailSection.MenuItems[0];
                    contentFrame.Navigate(typeof(DetailPage), selectedRecipe);
                }
            }
        }

        private void windowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {

            if (!isNarrow())
            {
                SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
                Window.Current.SizeChanged -= windowSizeChanged;
                systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                Frame.GoBack(new SuppressNavigationTransitionInfo());
            }
        }

        private bool isNarrow()
        {
            return Window.Current.Bounds.Width < 720;
        }

        private void navigateToPage(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            String itemName = args.InvokedItemContainer.Name;
            var preNavPageType = contentFrame.CurrentSourcePageType;

            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }

            if (itemName == "recipeContentView")
            {
                contentFrame.NavigateToType(typeof(DetailPage), selectedRecipe, navOptions);
            }
            else
            {
                contentFrame.NavigateToType(typeof(JournalPage), selectedRecipe, navOptions);
            }
        }
    }
}
