using Recipe_Book.Models;
using Recipe_Book.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.ViewModels
{
    /// <summary>
    /// The main application view model that controls the interactions
    /// between the UI and models behind the scenes.
    /// </summary>
    public class RecipeList
    {
        public static IdentifierGenerator recipeIdGenerator;
        public static IdentifierGenerator imageIdGenerator;
        public static IdentifierGenerator ingredientIdGenerator;
        public static IdentifierGenerator stepIdGenerator;

        private ObservableCollection<Recipe> recipes;
        private Recipe editingRecipe;
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
            recipes = new ObservableCollection<Recipe>();
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
        public RecipeList(ObservableCollection<Recipe> recipes)
        {
            this.recipes = recipes;
            selectedRecipe = 0;
            editing = false;
        }
        
        /// <summary>
        /// Gets the recipe list behind the scenes as a list.
        /// </summary>
        /// <returns>
        /// the list of recipes contained in this ViewModel
        /// </returns>
        public ObservableCollection<Recipe> getRecipeList()
        {
            return this.recipes;
        }

        /// <summary>
        /// Sets this recipe list to the given recipe list. If the
        /// given list is null, this recipe list will replaced with
        /// an ampty recipe list.
        /// </summary>
        /// <param name="newRecipeList">
        /// The new list of recipes to replace this list with
        /// </param>
        public void setRecipeList(ObservableCollection<Recipe> newRecipeList)
        {
            if (newRecipeList != null)
            {
                this.recipes = newRecipeList;
            } else
            {
                this.recipes = new ObservableCollection<Recipe>();
                // TODO: truncate all database tables
            }
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
            this.recipes.Add(newRecipe);
            RecipeBookDataAccessor.addRecipe(newRecipe);
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
            this.recipes.Remove(recipeToRemove);
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
            // TODO: handle the case when the index is out of bounds
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
    }
}
