﻿using Recipe_Book.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private ObservableCollection<String> units;
        private static int n;
        public SettingsPage()
        {
            this.InitializeComponent();
            n = 0;
            units = new ObservableCollection<String>(RecipeUtils.getUnitsOfMeasure());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base. OnNavigatedTo(e);
        }

        private async void tryEmptyBook(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog emptyConfirmationDialog = new ContentDialog
            {
                Title = "Permenantly empty recipe book?",
                Content = "If you empty your recipe book, you won't be" +
                " able to recover the recipes in it. Are you Sure you " +
                "want to empty your recipe book?",
                PrimaryButtonText = "Empty",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await emptyConfirmationDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

                // Delete recipes in database
                // Empty recipe list object
                // Delete all recipe images
                // Set the empty button to be disabled
                App.recipes.empty();
                Debug.WriteLine("Recipe book emptied");
            }
        }

        private void goBack(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            IList<PageStackEntry> backStack = Frame.BackStack;
            int backStackCount = backStack.Count;
            if (backStackCount > 0)
            {
                PageStackEntry masterPageEntry = backStack[backStackCount - 1];
                backStack.RemoveAt(backStackCount - 1);

                PageStackEntry modifiedEntry = new PageStackEntry(
                    masterPageEntry.SourcePageType,
                    null, null);

                backStack.Add(modifiedEntry);
            }
            Frame.GoBack();
        }
    }
}
