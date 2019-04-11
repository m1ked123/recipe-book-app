using Recipe_Book.Models;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecipeForm : Page
    {
        private RecipeList recipes;
        private Recipe recipe;
        private ObservableCollection<RecipeImage> images;
        private ObservableCollection<RecipeIngredient> ingredients;

        public RecipeForm()
        {
            this.InitializeComponent();
            ingredients = new ObservableCollection<RecipeIngredient>();
            this.recipeIngredients.ItemsSource = ingredients;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("Navigated to new detail page");

            recipes = (RecipeList)e.Parameter;
            if (recipes.isEditing())
            {
                recipe = recipes.getSelected();
                images = recipe.RecipeImages;
            } else
            {
                // we're creating a new recipe
                recipe = new Recipe();
                images = new ObservableCollection<RecipeImage>();
            }
            this.imagesSection.ItemsSource = images;
        }

        private void saveRecipe(object sender, RoutedEventArgs e)
        {
            String newRecipeName = this.recipeName.Text;
            double newRecipeRating = this.recipeRating.Value;

            recipe.Name = newRecipeName;
            recipe.Rating = newRecipeRating;
            recipe.LastMade = "";
            recipe.setImages(images);

            if (!recipes.isEditing())
            {
                recipes.addRecipe(recipe);
                Debug.WriteLine(recipes.getRecipeList().Count);
            } else
            {
                recipes.setEditing(false);
            }
            Frame.GoBack();
        }

        private void cancelRecipeCreation(object sender, RoutedEventArgs e)
        {
            if (recipes.isEditing())
            {
                recipes.setEditing(false);
            }
            Frame.GoBack();
        }

        private async void addImage(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile imageFile = await picker.PickSingleFileAsync();
            if (imageFile != null)
            {
                Uri imageUri = new Uri(imageFile.Path, UriKind.Absolute);
                RecipeImage newImage = null;
                BitmapImage addedImage = null;
                if (imageFile.IsAvailable)
                {
                    using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
                    {
                        addedImage = new BitmapImage();
                        await addedImage.SetSourceAsync(stream);
                        newImage = new RecipeImage(addedImage);
                        stream.Dispose();
                    }
                } else
                {
                    addedImage = new BitmapImage();
                    addedImage.UriSource = imageUri;
                    newImage = new RecipeImage(addedImage);
                }
                
                images.Add(newImage);
            }
        }

        private void addIngredient(object sender, RoutedEventArgs e)
        {
            ingredients.Add(new RecipeIngredient(1.0f, "Cups", "Flour"));
        }
    }
}
