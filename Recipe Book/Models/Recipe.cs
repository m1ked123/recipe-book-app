using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Recipe_Book.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the SQLite table used to store recipes
        /// </summary>
        public const String TABLE_NAME = "RECIPES";

        private String name;
        private long id;
        private double rating;
        private String lastMade;
        private ObservableCollection<RecipeImage> recipeImages;
        private ObservableCollection<RecipeIngredient> recipeIngredients;
        private ObservableCollection<RecipeStep> recipeSteps;

        public event PropertyChangedEventHandler PropertyChanged;

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                // PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public double Rating
        {
            get
            {
                return this.rating;
            }
            set
            {
                if (value >= 0 && value < 6)
                {
                    this.rating = value;
                    // PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rating"));
                }
            }
        }

        public String LastMade
        {
            get
            {
                return this.lastMade;
            }
            set
            {
                this.lastMade = value;
            }
        }

        public long ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public ObservableCollection<RecipeImage> RecipeImages
        {
            get
            {
                return this.recipeImages;
            }
            set
            {
                this.recipeImages = value;
            }
        }

        /// <summary>
        /// Gets and sets the list of steps needed to make this recipe.
        /// </summary>
        public ObservableCollection<RecipeStep> RecipeSteps
        {
            get
            {
                return this.getSteps();
            }
            set
            {
                this.setSteps(value);
            }
        }

        public ObservableCollection<RecipeIngredient> RecipeIngredients
        {
            get
            {
                return this.recipeIngredients;
            }
        }


        // TODO: consider cleaning up these constructors
        public Recipe() : this("New Recipe") { }

        public Recipe(String name) : this(name, -1) { }

        public Recipe(String name, long id) : this(name, id, 0) { }

        public Recipe(String name, long id, double rating) : this(name, id, rating, "Never") { }

        public Recipe(String name, long id, double rating, String lastMade)
        {
            this.name = name;
            this.id = id;
            this.rating = rating;
            this.lastMade = lastMade;
            this.recipeImages = new ObservableCollection<RecipeImage>();
            this.recipeIngredients = new ObservableCollection<RecipeIngredient>();
            this.recipeSteps = new ObservableCollection<RecipeStep>();
        }

        public void addImage(RecipeImage newImage)
        {
            this.recipeImages.Add(newImage);
        }

        public async void setImages(ObservableCollection<RecipeImage> newImages)
        {
            if (newImages != null)
            {
                for (int i = 0; i < newImages.Count; i++)
                {
                    RecipeImage newImage = newImages[i];
                    if (newImage.RecipeID == -1)
                    {
                        StorageFolder imageFolder = await RecipeList.imageFolder.CreateFolderAsync("" + this.id, CreationCollisionOption.OpenIfExists);
                        StorageFile imageFile = await StorageFile.GetFileFromPathAsync(newImage.ImagePath);
                        if (imageFile != null)
                        {
                            BitmapImage image = null;
                            StorageFile savingImage = await imageFile.CopyAsync(imageFolder);

                            if (imageFile.IsAvailable)
                            {
                                using (IRandomAccessStream stream = await savingImage.OpenAsync(FileAccessMode.Read))
                                {
                                    image = new BitmapImage();
                                    await image.SetSourceAsync(stream);
                                    newImage.setInternalImage(image);
                                    stream.Dispose();
                                }
                            }
                            else
                            {
                                image = new BitmapImage();
                                image.UriSource = new Uri(savingImage.Path);
                                newImage.setInternalImage(image);
                            }
                            newImage.ImagePath = savingImage.Path;
                        }

                        newImage.setRecipeId(this.id);
                        RecipeBookDataAccessor.addImage(newImage);
                    }
                }
                this.recipeImages = newImages;
            }
        }

        /// <summary>
        /// Sets the ingredients of this recipe to the given list of
        /// ingredients. 
        /// </summary>
        /// <param name="newIngredients">
        /// the list of ingredients to change the current list of 
        /// ingredients to
        /// </param>
        public void setIngredients(ObservableCollection<RecipeIngredient> newIngredients)
        {
            if (newIngredients != null)
            {
                for (int i = 0; i < newIngredients.Count; i++)
                {
                    RecipeIngredient ingredient = newIngredients[i];
                    if (ingredient.getRecipeId() == -1)
                    {
                        ingredient.setRecipeId(this.id);
                        RecipeBookDataAccessor.addIngredient(ingredient);
                    }
                }
                this.recipeIngredients = newIngredients;
            }
        }

        /// <summary>
        /// Gets a list of the steps needed to make this recipe.
        /// </summary>
        /// <returns>
        /// A list of the steps needed to make this recipe
        /// </returns>
        public ObservableCollection<RecipeStep> getSteps()
        {
            return this.recipeSteps;
        }

        /// <summary>
        /// Sets the steps of this recipe to the given list of recipe
        /// steps. This list cannot be null.
        /// </summary>
        /// <param name="newRecipeSteps">
        /// A non-null list of recipe steps
        /// </param>
        public void setSteps(ObservableCollection<RecipeStep> newRecipeSteps)
        {
            if (newRecipeSteps != null)
            {
                for (int i = 0; i < newRecipeSteps.Count; i++)
                {
                    RecipeStep step = newRecipeSteps[i];
                    if (step.RecipeID == -1)
                    {
                        step.setRecipeId(this.id);
                        step.setOrder(i);
                        RecipeBookDataAccessor.addStep(step);
                    } else
                    {
                        if (step.Order != i)
                        {
                            step.setOrder(i);
                        }
                        RecipeBookDataAccessor.updateStep(step);
                    }
                }
                this.recipeSteps = newRecipeSteps;
            } 
        }
    }
}
