using Recipe_Book.Models;
using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
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
        private RecipeJournal journalEntries;
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

        private async Task<bool> tryDeleteItem(String title, String content,
            String primaryButton)
        {
            ContentDialog deleteConfirmationDialog = 
                getDialog(title, content, primaryButton);

            ContentDialogResult result = await deleteConfirmationDialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        private async void deleteEntry(object sender, RoutedEventArgs e)
        {
            RecipeJournalEntry entryToRemove = (RecipeJournalEntry)((MenuFlyoutItem)e.OriginalSource).DataContext;
            String title = "Permenantly delete entry?";
            String content = "If you delete this entry, you won't be" +
                " able to recover it. Are you sure you want to delete" +
                " it?";
            String primaryButtonText = "Delete";
            bool result = await tryDeleteItem(title, content, primaryButtonText);
            if (result)
            {
                recipe.removeJournalEntry(entryToRemove);
            }
        }

        private async void emptyJournal (object sender, RoutedEventArgs e)
        {
            String title = "Permenantly empty journal?";
            String content = "If you empty this journal, you won't be" +
                " able to recover it. Are you sure you want to empty" +
                " the journal?";
            String primaryButtonText = "Empty";
            bool result = await tryDeleteItem(title, content, primaryButtonText);
            if (result)
            {
                recipe.emptyJournalEntries();
            }
        }

        private ContentDialog getDialog(String title, String content,
            String primaryButton)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = title;
            dialog.Content = content;
            dialog.PrimaryButtonText = primaryButton;
            dialog.CloseButtonText = "Cancel";
            return dialog;
        }
    }
}
