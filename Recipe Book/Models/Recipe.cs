using Recipe_Book.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
        public Recipe() : this("New Recipe") {}

        public Recipe(String name) : this(name, -1) {}

        public Recipe(String name, long id) : this(name, id, 0) {}

        public Recipe(String name, long id, double rating) : this(name, id, rating, "Never") {}

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

        public void addImage(String imagePath)
        {
            this.recipeImages.Add(new RecipeImage(imagePath));
        }

        public void addImage(RecipeImage newImage)
        {
            this.recipeImages.Add(newImage);
        }

        public void setImages(ObservableCollection<RecipeImage> newImages)
        {
            this.recipeImages = newImages;
        }

        public void setIngredients(ObservableCollection<RecipeIngredient> newIngredients)
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
                this.recipeSteps = newRecipeSteps;
            }
        }
    }
}
