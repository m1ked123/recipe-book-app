using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using Recipe_Book.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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

        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recipe = (Recipe)e.Parameter;
        }

        private void madeToday(object sender, RoutedEventArgs e)
        {
            long newId = RecipeList.journalEntryIdGenerator.getId();
            RecipeJournalEntry madeTodayEntry = new RecipeJournalEntry(newId);
            madeTodayEntry.setEntryDate(DateTime.Now);
            madeTodayEntry.setEntryNotes("Added as quick entry");
            madeTodayEntry.setRecipeId(recipe.ID);
            recipe.addJournalEntry(madeTodayEntry);
            
            FlyoutBase.ShowAttachedFlyout(this.mainContent);
        }
    }
}
