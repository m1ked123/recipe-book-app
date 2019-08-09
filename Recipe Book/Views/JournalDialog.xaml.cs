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
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book.Views
{
    public sealed partial class JournalDialog : ContentDialog
    {
        private RecipeJournalEntry newEntry;

        public RecipeJournalEntry NewEntry
        {
            get
            {
                return newEntry;
            }
        }

        public JournalDialog()
        {
            this.InitializeComponent();
        }

        public JournalDialog(RecipeJournalEntry updatingEntry)
        {
            this.InitializeComponent();
            newEntry = updatingEntry;
            entryDatePicker.Date = newEntry.EntryDate;
            entryNotesControl.Text = newEntry.EntryNotes;
            entryRatingControl.Value = newEntry.Rating;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            long newId = RecipeList.journalEntryIdGenerator.getId();
            newEntry = new RecipeJournalEntry(newId);
            newEntry.setEntryDate(entryDatePicker.Date.Value);
            newEntry.setEntryNotes(entryNotesControl.Text);
            newEntry.setRating(entryRatingControl.Value);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
