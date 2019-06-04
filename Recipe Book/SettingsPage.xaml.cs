using Recipe_Book.Utils;
using System;
using System.Collections.ObjectModel;
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

            this.unitsOfMeasureList.ItemsSource = units;
        }

        private void addUnitOfMeasure(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.units.Add("New unit: " + n);
            n++;
        }
    }
}
