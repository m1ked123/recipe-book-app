using Recipe_Book.Models;
using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class IngredientDialog : ContentDialog
    {
        private RecipeIngredient newIngredient;

        public RecipeIngredient NewIngredient
        {
            get
            {
                return this.newIngredient;
            }
            private set { }
        }

        public IngredientDialog()
        {
            this.InitializeComponent();
        }

        public IngredientDialog(RecipeIngredient editingIngredient)
        {
            this.InitializeComponent();
            this.newIngredient = editingIngredient;
            this.ingredientName.Text = editingIngredient.IngredientName;
            this.ingredientQuant.Text = editingIngredient.Quantity;
            this.units.Text = editingIngredient.UnitOfMeasure;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool canAddIngredient = true;
            this.dialogErrorText.Text = "";
            String ingredientNameText = this.ingredientName.Text;
            if (ingredientNameText.Length == 0)
            {
                args.Cancel = true;
                this.dialogErrorText.Text += "Ingredient Name cannot be empty";
                canAddIngredient = false;
            }

            String ingredientQuantity = this.ingredientQuant.Text;
            if (ingredientQuantity.Length == 0)
            {
                args.Cancel = true;
                this.dialogErrorText.Text += "\nIngredient quantity cannot be empty.";
                canAddIngredient = false;
            }

            String UOM = this.units.Text;
            if (UOM.Length == 0)
            {
                this.units.Text = "-";
            }

            if (canAddIngredient)
            {
                if (this.newIngredient == null)
                {
                    long id = RecipeList.ingredientIdGenerator.getId();
                    newIngredient = new RecipeIngredient(id,
                        ingredientQuantity, UOM, ingredientNameText);
                } else
                {
                    newIngredient.setIngredientName(ingredientNameText);
                    newIngredient.setQuantity(ingredientQuantity);
                    newIngredient.setUnitOfMeasure(UOM);
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
