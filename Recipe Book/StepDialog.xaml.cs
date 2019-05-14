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

namespace Recipe_Book
{
    public sealed partial class StepDialog : ContentDialog
    {
        private RecipeStep newRecipeStep;

        public RecipeStep NewRecipeStep
        {
            get
            {
                return this.newRecipeStep;
            }
            private set { }
        }

        public StepDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            String stepDescriptionText = this.stepDescription.Text;
            if (stepDescriptionText.Length > 0)
            {
                long id = RecipeList.stepIdGenerator.getId();
                newRecipeStep = new RecipeStep(id, 0, stepDescriptionText);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
