using Recipe_Book.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.ViewModels
{
    class RecipeList
    {
        private ObservableCollection<Recipe> recipes;

        public RecipeList()
        {
            this.recipes = new ObservableCollection<Recipe>();
        }

        public RecipeList(ObservableCollection<Recipe> recipes)
        {
            this.recipes = recipes;
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
    }
}
