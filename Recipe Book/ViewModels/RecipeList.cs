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

        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                return this.recipes;
            }
        }

        public Recipe EditingRecipe
        {
            get; set;
        }

        public RecipeList()
        {
            this.recipes = new ObservableCollection<Recipe>();
            this.selectedRecipe = 0;
            this.editing = false;
        }

        public RecipeList(ObservableCollection<Recipe> recipes)
        {
            this.recipes = recipes;
            this.selectedRecipe = 0;
            this.editing = false;
        }

        public void setRecipeList(ObservableCollection<Recipe> newRecipeList)
        {
            this.recipes = newRecipeList;
        }

        public ObservableCollection<Recipe> getRecipeList()
        {
            return this.recipes;
        }

        public void addRecipe(Recipe newRecipe)
        {
            this.recipes.Add(newRecipe);
        }

        public void removeRecipe(Recipe recipeToRemove)
        {
            this.recipes.Remove(recipeToRemove);
            RecipeBookDataAccessor.deleteRecipe(recipeToRemove);
        }

        public void setSelected(int selectedIndex)
        {
            this.selectedRecipe = selectedIndex;
        }

        public Recipe getSelected()
        {
            return this.recipes[this.selectedRecipe];
        }

        public int getSelectedIndex()
        {
            return this.selectedRecipe;
        }

        public bool isEditing()
        {
            return this.editing;
        }

        public void setEditing(bool isEditing)
        {
            this.editing = isEditing;
        }
    }
}
