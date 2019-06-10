using Recipe_Book.Models;
using Recipe_Book.Utils;
using System.Collections.ObjectModel;
using Windows.Storage;
using System;

namespace Recipe_Book.ViewModels
{
    /// <summary>
    /// Represents a list of Recipes. A recipe list allows for access
    /// of recipes including adding, removing, and retrieving a 
    /// recipe.
    /// </summary>
    public class RecipeList
    {
        public static IdentifierGenerator recipeIdGenerator;
        public static IdentifierGenerator imageIdGenerator;
        public static IdentifierGenerator ingredientIdGenerator;
        public static IdentifierGenerator stepIdGenerator;

        public static StorageFolder imageFolder;
        public static StorageFolder tempFolder;

        private const int DEFAULT_SIZE = 10;

        private int size;
        private Recipe[] recipes;
        private int selectedRecipe;
        private bool editing;

        /// <summary>
        /// Gets and sets the recipes that are in this recipe list.
        /// The list cannot be set to null.
        /// </summary>
        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                return getRecipeList();
            }
            set
            {
                setRecipeList(value);
            }
        }

        /// <summary>
        /// The default constructor that initializes this ViewModel
        /// to an empty list of recipes.
        /// </summary>
        public RecipeList()
        {
            recipes = new Recipe[DEFAULT_SIZE];
            size = 0;
            selectedRecipe = 0;
            editing = false;
        }

        /// <summary>
        /// An alternative constructor that initializes this ViewModel
        /// to the given list of recipes.
        /// </summary>
        /// <param name="recipes">
        /// a list of recipes to use for this view model
        /// </param>
        public RecipeList(Recipe[] recipes)
        {
            this.recipes = recipes;
            this.size = recipes.Length;
            selectedRecipe = 0;
            editing = false;
        }

        /// <summary>
        /// Gets the recipe list behind the scenes as an observable
        /// collection.
        /// </summary>
        /// <returns>
        /// an observable list of recipes in this recipe list
        /// </returns>
        public ObservableCollection<Recipe> getRecipeList()
        {
            ObservableCollection<Recipe> result = new ObservableCollection<Recipe>();
            for (int i = 0; i < size; i++)
            {
                result.Add(recipes[i]);
            }
            return result;
        }

        /// <summary>
        /// Sets this recipe list to the given recipe list. If the
        /// given list is null, this recipe list will remain unchanged.
        /// </summary>
        /// <param name="newRecipeList">
        /// The new list of recipes to replace this list with
        /// </param>
        public void setRecipeList(ObservableCollection<Recipe> newRecipeList)
        {
            if (newRecipeList != null)
            {
                int newRecipeCount = newRecipeList.Count;
                if (recipes.Length < newRecipeCount)
                {
                    int growthFactor = newRecipeCount - recipes.Length;
                    increseArraySize(growthFactor);
                }
                for (int i = 0; i < newRecipeCount; i++)
                {
                    recipes[i] = newRecipeList[i];
                }
                this.size = newRecipeCount;
            }
        }

        // Increses the size of the recipe list by a fixed number
        private void increseArraySize(int growthFactor)
        {
            Recipe[] newRecipeList = new Recipe[recipes.Length + growthFactor];
            for (int i = 0; i < this.size; i++)
            {
                newRecipeList[i] = this.recipes[i];
            }
            this.recipes = newRecipeList;
        }

        /// <summary>
        /// Adds the given recipe to the recipe list. Also saves the
        /// new recipe to the app database.
        /// </summary>
        /// <param name="newRecipe">
        /// the new recipe to add
        /// </param>
        public void addRecipe(Recipe newRecipe)
        {
            if (this.recipes.Length <= this.size)
            {
                increseArraySize(100);
            }
            this.recipes[this.size] = newRecipe;
            RecipeBookDataAccessor.addRecipe(newRecipe);
            this.size++;
        }

        /// <summary>
        /// Removes the given recipe from the recipe list and deletes
        /// it from teh app database
        /// </summary>
        /// <param name="recipeToRemove">
        /// the recipe to delete from the list
        /// </param>
        public void removeRecipe(Recipe recipeToRemove)
        {
            // this.recipes.Remove(recipeToRemove);
            RecipeBookDataAccessor.deleteRecipe(recipeToRemove);
        }

        /// <summary>
        /// Sets the index of the selected recipe to the given index
        /// </summary>
        /// <param name="selectedIndex">
        /// the new index of the selecteed recipe
        /// </param>
        public void setSelected(int selectedIndex)
        {
            // TODO: handle the case when the index is out of bounds
            this.selectedRecipe = selectedIndex;
        }

        /// <summary>
        /// Gets the recipe that is currently selected in the list
        /// </summary>
        /// <returns>
        /// the recipe that is currently selected in the list
        /// </returns>
        public Recipe getSelected()
        {
            if (this.selectedRecipe < 0 || this.size == 0)
            {
                return null;
            }
            return this.recipes[this.selectedRecipe];
        }

        /// <summary>
        /// Gets the index in the list that is currently selected
        /// </summary>
        /// <returns>
        /// gets the index of the currently selected recipe
        /// </returns>
        public int getSelectedIndex()
        {
            return this.selectedRecipe;
        }

        /// <summary>
        /// Returns if the recipe list is currently being edited or
        /// not.
        /// </summary>
        /// <returns>
        /// true if an item in the list is being edited, false otherwise
        /// </returns>
        public bool isEditing()
        {
            return this.editing;
        }

        /// <summary>
        /// Sets the editing state of the list to the given value
        /// </summary>
        /// <param name="isEditing">
        /// if the list is in an edit session or not
        /// </param>
        public void setEditing(bool isEditing)
        {
            this.editing = isEditing;
        }

        /// <summary>
        /// Empties this recipe list and deletes all related 
        /// information
        /// </summary>
        public async void empty()
        {
            this.recipes = new Recipe[this.recipes.Length];
            RecipeBookDataAccessor.emptyRecipteList();
            selectedRecipe = -1;
            recipeIdGenerator.reset();
            imageIdGenerator.reset();
            ingredientIdGenerator.reset();
            stepIdGenerator.reset();
            await imageFolder.DeleteAsync();
        }
    }
}
