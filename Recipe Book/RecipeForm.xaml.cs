using Recipe_Book.Models;
using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.Generic;
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
        private StorageFolder tempImageFolder;

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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
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

                for (int i = 0; i < recipe.RecipeImages.Count; i++)
                {
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
                recipe.ID = RecipeList.recipeIdGenerator.getId();
            }

            tempImageFolder = await RecipeList.tempFolder.CreateFolderAsync("" + recipe.ID, CreationCollisionOption.OpenIfExists);

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
                recipes.addRecipe(recipe);
                recipes.setSelected(recipes.getRecipeList().Count - 1);
            }
            else
            {
                recipes.setEditing(false);
                RecipeBookDataAccessor.updateRecipe(recipe);
            }

            IList<PageStackEntry> backStack = Frame.BackStack;
            int backStackCount = backStack.Count;
            if (backStackCount > 0)
            {
                PageStackEntry masterPageEntry = backStack[backStackCount - 1];
                backStack.RemoveAt(backStackCount - 1);

                PageStackEntry modifiedEntry = new PageStackEntry(
                    masterPageEntry.SourcePageType,
                    recipes.getSelectedIndex(),
                    masterPageEntry.NavigationTransitionInfo
                    );

                backStack.Add(modifiedEntry);
            }

            Frame.GoBack();
        }

        /*
         * Do not save any changes and exit the edit session.
         */
        private async void cancelRecipeCreation(object sender, RoutedEventArgs e)
        {
            if (recipes.isEditing())
            {
                recipes.setEditing(false);
            }
            await tempImageFolder.DeleteAsync(); // delete all temp files added
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
                StorageFile tempImage = await imageFile.CopyAsync(tempImageFolder);
                Uri imageUri = new Uri(tempImage.Path, UriKind.Absolute);
                RecipeImage newImage = null;
                BitmapImage addedImage = null;
                if (tempImage.IsAvailable)
                {
                    using (IRandomAccessStream stream = await tempImage.OpenAsync(FileAccessMode.Read))
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

                newImage.ImagePath = imageUri.AbsoluteUri;
                Debug.WriteLine(newImage.ImagePath);
                images.Add(newImage);
                this.imageFlipView.SelectedIndex = this.images.Count - 1;
            }
        }

        /*
         * Adds a new ingredient to the ingredient list for this recipe
         * that's being added or edited in this form.
         */
        private async void addIngredient(object sender, RoutedEventArgs e)
        {
            IngredientDialog ingredientDialog = new IngredientDialog();
            await ingredientDialog.ShowAsync();

            if (ingredientDialog.NewIngredient != null)
            {
                RecipeIngredient newIngredient = ingredientDialog.NewIngredient;
                this.ingredients.Add(newIngredient);
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
                newRecipeStep.setOrder(this.steps.Count);
            }
        }

        /*
         * Deletes the recipe ingredient with the context menu that
         * was activated.
         */
        private void deleteIngredient(object sender, RoutedEventArgs e)
        {
            RecipeIngredient ingredientToRemove =
                (RecipeIngredient)((MenuFlyoutItem)e.OriginalSource).DataContext;
            ingredients.Remove(ingredientToRemove);
            if (ingredientToRemove.ID > 0)
            {
                RecipeBookDataAccessor.deleteIngredient(ingredientToRemove);
            }
        }

        private void deleteStep(object sender, RoutedEventArgs e)
        {
            RecipeStep stepToRemove =
                (RecipeStep)((MenuFlyoutItem)e.OriginalSource).DataContext;
            steps.Remove(stepToRemove);
            if (stepToRemove.ID > 0)
            {
                RecipeBookDataAccessor.deleteStep(stepToRemove);
            }
        }

        private void deleteImage(object sender, RoutedEventArgs e)
        {
            RecipeImage imageToRemove =
                (RecipeImage)((MenuFlyoutItem)e.OriginalSource).DataContext;
            images.Remove(imageToRemove);
            if (imageToRemove.ID > 0)
            {
                RecipeBookDataAccessor.deleteImage(imageToRemove);
            }
        }
    }
}
