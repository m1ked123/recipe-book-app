using Recipe_Book.Models;
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
        private ObservableCollection<Recipe> recipes;
        private int selectedRecipe;
        private bool editing;

        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                return this.recipes;
            }
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
        }

        public void setSelected(int selectedIndex)
        {
            this.selectedRecipe = selectedIndex;
        }

        public Recipe getSelected()
        {
            return this.recipes[this.selectedRecipe];
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
