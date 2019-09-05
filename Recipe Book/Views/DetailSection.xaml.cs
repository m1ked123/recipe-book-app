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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailSection : Page
    {
        private RecipeList recipes;

        public DetailSection()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recipes = (RecipeList)e.Parameter;

            detailSection.SelectedItem = detailSection.MenuItems[0];
            contentFrame.Navigate(typeof(DetailPage), recipes);
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
                contentFrame.NavigateToType(typeof(DetailPage), recipes, navOptions);
            }
            else
            {
                contentFrame.NavigateToType(typeof(JournalPage), recipes.getSelected(), navOptions);
            }
        }
    }
}
