using Recipe_Book.Models;
using Recipe_Book.Utils;
using System.Collections.ObjectModel;
using Windows.Storage;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Recipe_Book.ViewModels
{
    /// <summary>
    /// The main application view model that controls the interactions
    /// between the UI and models behind the scenes.
    /// </summary>
    public class RecipeList : INotifyPropertyChanged
    {
        public static IdentifierGenerator recipeIdGenerator;
        public static IdentifierGenerator imageIdGenerator;
        public static IdentifierGenerator ingredientIdGenerator;
        public static IdentifierGenerator stepIdGenerator;
        public static IdentifierGenerator journalEntryIdGenerator;

        public static StorageFolder imageFolder;
        public static StorageFolder tempFolder;

        private ObservableCollection<Recipe> recipes;
        private int selectedRecipeIndex;
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
        /// Gets or sets the selected index for the recipe list.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return getSelectedIndex();
            }
            set
            {
                setSelected(value);
                RaisePropertyChanged("SelectedIndex");
            }
        }

        /// <summary>
        /// The default constructor that initializes this ViewModel
        /// to an empty list of recipes.
        /// </summary>
        public RecipeList()
        {
            recipes = new ObservableCollection<Recipe>();
            selectedRecipeIndex = 0;
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
            selectedRecipeIndex = 0;
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
        /// given list is null, this function will throw an
        /// ArgumentNullException.
        /// </summary>
        /// <param name="newRecipeList">
        /// The new list of recipes to replace this list with
        /// </param>
        public void setRecipeList(ObservableCollection<Recipe> newRecipeList)
        {
            if (newRecipeList != null)
            {
                recipes = newRecipeList;
            }
            else
            {
                throw new ArgumentNullException();
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
            if (selectedIndex < 0 || selectedIndex > recipes.Count - 1)
            {
                throw new ArgumentOutOfRangeException("The given index" +
                    " is outside of the recipe list's range: " +
                    selectedIndex, new IndexOutOfRangeException());
            }
            selectedRecipeIndex = selectedIndex;
        }

        /// <summary>
        /// Gets the recipe that is currently selected in the list
        /// </summary>
        /// <returns>
        /// the recipe that is currently selected in the list
        /// </returns>
        public Recipe getSelected()
        {
            if (selectedRecipeIndex < 0 || recipes.Count == 0)
            {
                return null;
            }
            return recipes[selectedRecipeIndex];
        }

        /// <summary>
        /// Gets the index in the list that is currently selected
        /// </summary>
        /// <returns>
        /// gets the index of the currently selected recipe
        /// </returns>
        public int getSelectedIndex()
        {
            return selectedRecipeIndex;
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
            return editing;
        }

        /// <summary>
        /// Sets the editing state of the list to the given value
        /// </summary>
        /// <param name="isEditing">
        /// if the list is in an edit session or not
        /// </param>
        public void setEditing(bool isEditing)
        {
            editing = isEditing;
        }

        /// <summary>
        /// Gets if this recipe list is empty or not.
        /// </summary>
        /// <returns>
        /// True if there are recipes in this list. False otherwise.
        /// </returns>
        public bool isEmpty()
        {
            return recipes.Count == 0;
        }

        /// <summary>
        /// Empties this recipe list and deletes all related 
        /// information
        /// </summary>
        public void empty()
        {
            recipes.Clear();
            RecipeBookDataAccessor.emptyRecipteList();
            selectedRecipeIndex = -1;
            recipeIdGenerator.reset();
            imageIdGenerator.reset();
            ingredientIdGenerator.reset();
            stepIdGenerator.reset();
            verifyImageFolder();
        }

        public async void verifyImageFolder()
        {
            StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            String desiredName = "images";
            StorageFolder imageFolder =
                await localFolder.CreateFolderAsync(desiredName, CreationCollisionOption.OpenIfExists);
            StorageFolder tempImageFolder =
                await tempFolder.CreateFolderAsync(desiredName, CreationCollisionOption.OpenIfExists);
            RecipeList.imageFolder = imageFolder;
            RecipeList.tempFolder = tempImageFolder;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
