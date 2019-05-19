﻿using Recipe_Book.Models;
using Recipe_Book.ViewModels;
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
    // TODO: make this UI responsive
    // TODO: make the detail UI show nothing is the list is empty
    public sealed partial class MainPage : Page
    {
        private RecipeList recipes;
        public MainPage()
        {
            this.InitializeComponent();
            recipes = App.recipes;
            this.recipeListView.ItemsSource = recipes.getRecipeList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int index = -1;
            if (e.Parameter == null)
            {
                int numRecipes = recipes.getRecipeList().Count;
                if (numRecipes != 0)
                {
                    index = 0;
                }
            }
            else
            {
                index = (int)e.Parameter;
            }

            this.recipeListView.SelectedIndex = index;
            this.recipes.setSelected(index);

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

        private void editRecipe(object sender, RoutedEventArgs e)
        {
            Recipe clickedRecipe = (Recipe)((MenuFlyoutItem)e.OriginalSource).DataContext;
            int editingRecipe = recipes.Recipes.IndexOf(clickedRecipe);
            recipes.setSelected(editingRecipe);
            recipes.setEditing(true);
            Frame.Navigate((typeof(RecipeForm)), recipes);
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
            Recipe selectedRecipe = recipes.getSelected();
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
            }
        }
    }
}
