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
    /// <summary>
    /// Class <code>Recipe</code> represents a recipe. Recipes have
    /// a name, a collection of images, a collection of ingredients,
    /// and a collection of steps that describe how to make it.
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// The name of the SQLite table used to store recipes
        /// </summary>
        public const String TABLE_NAME = "RECIPES";

        private String name; // recipe name
        private long id; // recipe's id for database
        private double rating; // recipe rating
        private DateTime lastMade;
        private ObservableCollection<RecipeImage> recipeImages;
        private ObservableCollection<RecipeIngredient> recipeIngredients;
        private ObservableCollection<RecipeStep> recipeSteps;
        private ObservableCollection<RecipeJournalEntry> journalEntries;
        // TODO: add property to get the most recent journal entry.

        public DateTime LastMade
        {
            get
            {
                return lastMade;
            }
            set
            {
                lastMade = value;
            }
        } 

        /// <summary>
        /// Gets or sets the name of this recipe.
        /// </summary>
        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the rating for this recipe. The rating must
        /// be between 1 and 5.
        /// </summary>
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
                }
            }
        }

        /// <summary>
        /// Gets or sets the ID of this recipe.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the collection of images associated with this
        /// recipe.
        /// </summary>
        public ObservableCollection<RecipeImage> RecipeImages
        {
            get
            {
                return getImages();
            }
            set
            {
                setImages(value);
            }
        }

        /// <summary>
        /// Gets and sets the list of steps needed to make this recipe.
        /// </summary>
        public ObservableCollection<RecipeStep> RecipeSteps
        {
            get
            {
                return getSteps();
            }
            set
            {
                setSteps(value);
            }
        }

        /// <summary>
        /// Gets the ingredients that are associated with this recipe
        /// </summary>
        public ObservableCollection<RecipeIngredient> RecipeIngredients
        {
            get
            {
                return this.recipeIngredients;
            }
        }

        public ObservableCollection<RecipeJournalEntry> JournalEntries
        {
            get
            {
                return this.journalEntries;
            }
        }

        /// <summary>
        /// Creates a new recipe with a default id and name.
        /// </summary>
        public Recipe() : this(-1) { }

        /// <summary>
        /// Creates a new recipe witht he given id and a default name.
        /// </summary>
        /// <param name="id">
        /// the ID for this recipe
        /// </param>
        public Recipe(long id) : this(id, "New Recipe") { }
        
        /// <summary>
        /// Creates a recipe with the given ID and name. It won't
        /// have a rating.
        /// </summary>
        /// <param name="id">
        /// The ID for this recipe.
        /// </param>
        /// <param name="name">
        /// The name for this recipe.
        /// </param>
        public Recipe(long id, String name) : this(id, name, 0) { }

        /// <summary>
        /// Creates a recipe with the given id, name, and rating
        /// </summary>
        /// <param name="id">
        /// The ID for this recipe.
        /// </param>
        /// <param name="name">
        /// The name for this recipe.
        /// </param>
        /// <param name="rating">
        /// The rating for this recipe.
        /// </param>
        public Recipe(long id, String name, double rating)
        {
            this.name = name;
            this.id = id;
            this.rating = rating;
            this.recipeImages = new ObservableCollection<RecipeImage>();
            this.recipeIngredients = new ObservableCollection<RecipeIngredient>();
            this.recipeSteps = new ObservableCollection<RecipeStep>();
            this.journalEntries = new ObservableCollection<RecipeJournalEntry>();
        }

        public void addJournalEntry(RecipeJournalEntry newEntry)
        {
            if (newEntry != null)
            {
                journalEntries.Add(newEntry);
                RecipeBookDataAccessor.addJournalEntry(newEntry);
                DateTime entryDate = newEntry.EntryDate.DateTime;
                if (entryDate > lastMade)
                {
                    lastMade = entryDate;
                }
            }
        }

        public void updateJournalEntry(RecipeJournalEntry updatedEntry)
        {
            if (updatedEntry != null && updatedEntry.ID != -1)
            {
                int entryIndex = journalEntries.IndexOf(updatedEntry);
                journalEntries[entryIndex] = updatedEntry;
                RecipeBookDataAccessor.updateJournalEntry(updatedEntry);
            }
        }

        public void setJournalEntries(ObservableCollection<RecipeJournalEntry> entries)
        {
            this.journalEntries = entries;
            for (int i = 0; i < entries.Count; i++)
            {
                DateTime entryDate = entries[i].EntryDate.DateTime;
                if (entryDate > lastMade)
                {
                    lastMade = entryDate;
                }
            }
        }

        public void removeJournalEntry(RecipeJournalEntry entryToRemove)
        {
            journalEntries.Remove(entryToRemove);
            RecipeBookDataAccessor.deleteJournalEntry(entryToRemove);
        }

        /// <summary>
        /// Adds the given recipe image to the recipe. The image cannot
        /// be null.
        /// </summary>
        /// <param name="newImage">
        /// the new image that will be associated with this recipe
        /// </param>
        public void addImage(RecipeImage newImage)
        {
            if (newImage != null)
            {
                this.recipeImages.Add(newImage);
            } else
            {
                throw new ArgumentNullException("The recipe image being" +
                    " added cannot be null", new ArgumentNullException());
            }
        }

        /// <summary>
        /// Sets the associated images of this recipe to the given new
        /// set of images.
        /// </summary>
        /// <param name="newImages">
        /// The new images to associate to this recipe.
        /// </param>
        public async void setImages(ObservableCollection<RecipeImage> newImages)
        {
            // TODO: clean up this code a bit, it's messy
            if (newImages != null)
            {
                recipeImages.Clear();
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
                    recipeImages.Add(newImage);
                }
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
                    } else
                    {
                        RecipeBookDataAccessor.updateIngredient(ingredient);
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
        /// Gets the list of images that are associated to this recipe
        /// </summary>
        /// <returns>
        /// The list of images associated to this recipe
        /// </returns>
        public ObservableCollection<RecipeImage> getImages()
        {
            return recipeImages;
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
