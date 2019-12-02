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
        private RecipeJournalEntry entry;

        public RecipeJournalEntry NewEntry
        {
            get
            {
                return entry;
            }
        }

        public JournalDialog() : this(null) { }

        public JournalDialog(RecipeJournalEntry targetEntry)
        {
            this.InitializeComponent();
            entry = targetEntry;
            if (entry != null)
            {
                entryDatePicker.Date = entry.EntryDate;
                entryNotesControl.Text = entry.EntryNotes;
                entryRatingControl.Value = entry.Rating;
            }
            entryDatePicker.MaxDate = DateTimeOffset.Now.Date;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (entry == null)
            {
                long newId = RecipeList.journalEntryIdGenerator.getId();
                entry = new RecipeJournalEntry(newId);
            }
            entry.setEntryDate(entryDatePicker.Date.Value);
            entry.setEntryNotes(entryNotesControl.Text);
            entry.setRating(entryRatingControl.Value);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
