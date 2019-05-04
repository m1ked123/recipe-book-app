using Recipe_Book.Models;
using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Recipe_Book
{
    /// <summary>
    /// This page is used for editing existing and creating new recipes
    /// </summary>
    public sealed partial class RecipeForm : Page
    {
        private RecipeList recipes; // The recipe list ViewModel
        private Recipe recipe; // the recipe being edited/created in this form
        private Random r;
        private ObservableCollection<RecipeImage> images;
        private ObservableCollection<RecipeIngredient> ingredients;
        private ObservableCollection<RecipeStep> steps;

        public RecipeForm()
        {
            this.InitializeComponent();
            r = new Random();
            images = new ObservableCollection<RecipeImage>();
            ingredients = new ObservableCollection<RecipeIngredient>();
            steps = new ObservableCollection<RecipeStep>();
        }

        /*  
         *  When the page is navigated to, check if we're editing an
         *  Existing recipe. If so, load that one. Otherwise, create
         *  a new recipe and use that as the context for this form.
         */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recipes = (RecipeList)e.Parameter;
            if (recipes.isEditing())
            {
                recipe = recipes.getSelected();
                for (int i = 0; i < recipe.RecipeIngredients.Count; i++)
                {
                    ingredients.Add(recipe.RecipeIngredients[i]);
                }

                for (int i = 0; i < recipe.RecipeImages.Count; i++) {
                    images.Add(recipe.RecipeImages[i]);
                }
                
                for (int i = 0; i < recipe.RecipeSteps.Count; i++)
                {
                    steps.Add(recipe.RecipeSteps[i]);
                }
            }
            else
            {
                // we're creating a new recipe
                recipe = new Recipe(); 
            }
            this.imageFlipView.ItemsSource = images;
            this.ingredientList.ItemsSource = ingredients;
            this.recipeSteps.ItemsSource = steps;
        }

        /*
         * Save the changes made to an existing recipe or add the new
         * recipe to the recipe list.
         */
        private void saveRecipe(object sender, RoutedEventArgs e)
        {
            String newRecipeName = this.recipeName.Text;
            double newRecipeRating = this.recipeRating.Value;

            recipe.Name = newRecipeName;
            recipe.Rating = newRecipeRating;
            recipe.LastMade = "";
            recipe.setImages(images);
            recipe.setIngredients(ingredients);
            recipe.setSteps(steps);

            if (!recipes.isEditing())
            {
                recipe.ID = RecipeList.recipeIdGenerator.getId();
                recipes.addRecipe(recipe);
                RecipeBookDataAccessor.addRecipe(recipe);
            }
            else
            {
                recipes.setEditing(false);
                RecipeBookDataAccessor.updateRecipe(recipe);
            }
            Frame.GoBack();
        }

        /*
         * Do not save any changes and exit the edit session.
         */
        private void cancelRecipeCreation(object sender, RoutedEventArgs e)
        {
            if (recipes.isEditing())
            {
                recipes.setEditing(false);
            }
            Frame.GoBack();
        }

        /*
         * Add a new image to the recipe's image list. The image is
         * selected by teh user from a file dialog.
         */
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
                }
                else
                {
                    addedImage = new BitmapImage();
                    addedImage.UriSource = imageUri;
                    newImage = new RecipeImage(addedImage);
                }

                images.Add(newImage);
                this.imageFlipView.SelectedIndex = this.images.Count - 1;
            }
        }

        private async void addIngredient(object sender, RoutedEventArgs e)
        {
            IngredientDialog ingredientDialog = new IngredientDialog();
            await ingredientDialog.ShowAsync();

            if (ingredientDialog.NewIngredient != null)
            {
                Debug.WriteLine(ingredientDialog.NewIngredient);
                this.ingredients.Add(ingredientDialog.NewIngredient);
            }
        }

        private async void addStep(object sender, RoutedEventArgs e)
        {
            StepDialog stepDialog = new StepDialog();
            await stepDialog.ShowAsync();
            RecipeStep newRecipeStep = stepDialog.NewRecipeStep;
            if (newRecipeStep != null)
            {
                this.steps.Add(newRecipeStep);
            }
        }
    }
}
