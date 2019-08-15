using Recipe_Book.Models;
using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalPage : Page
    {
        private ObservableCollection<RecipeJournalEntry> journalEntries;
        private Recipe recipe;
        public JournalPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recipe = (Recipe)e.Parameter;
            journalEntries = recipe.JournalEntries;
        }


        private async void showJournalDialog(object sender, RoutedEventArgs e)
        {
            JournalDialog journalDialog = new JournalDialog();
            await journalDialog.ShowAsync();

            if (journalDialog.NewEntry != null)
            {
                journalDialog.NewEntry.RecipeID = recipe.ID;
                recipe.addJournalEntry(journalDialog.NewEntry);
            }
        }

        private async void editEntry(object sender, RoutedEventArgs e)
        {
            RecipeJournalEntry editingEntry = (RecipeJournalEntry)((MenuFlyoutItem)e.OriginalSource).DataContext;
            JournalDialog journalDialog = new JournalDialog(editingEntry);
            await journalDialog.ShowAsync();
            recipe.updateJournalEntry(journalDialog.NewEntry);
        }

        private async Task<bool> tryDeleteItem(String itemLabel)
        {
            ContentDialog deleteConfirmationDialog = new ContentDialog
            {
                Title = "Permenantly delete " + itemLabel + "?",
                Content = "If you delete this " + itemLabel + ", you won't be" +
                " able to recover it. Are you sure you want to delete" +
                " it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        private async void deleteEntry(object sender, RoutedEventArgs e)
        {
            RecipeJournalEntry entryToRemove = (RecipeJournalEntry)((MenuFlyoutItem)e.OriginalSource).DataContext;
            bool result = await tryDeleteItem("entry");
            if (result)
            {
                recipe.removeJournalEntry(entryToRemove);
            }
        }
    }
}
