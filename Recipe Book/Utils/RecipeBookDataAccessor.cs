using Microsoft.Data.Sqlite;
using Recipe_Book.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Utils
{
    public class RecipeBookDataAccessor
    {
        public const String RECIPE_TABLE_NAME = "RECIPES";
        public const String INGREDIENT_TABLE_NAME = "INGREDIENTS";
        public const String IMAGE_TABLE_NAME = "IMAGES";
        public const String STEP_TABLE_NAME = "STEPS";

        public static void InitializeDatabase()
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            try
            {
                db.Open();

                String createRecipeDbText = "CREATE TABLE IF NOT " +
                    "EXISTS RECIPES (ID INTEGER PRIMARY KEY, NAME " +
                    "VARCHAR(500), RATING DOUBLE)";
                SqliteCommand createRecipeDb = new SqliteCommand(createRecipeDbText, db);
                createRecipeDb.ExecuteReader();

                String createImageDbText = "CREATE TABLE IF NOT EXISTS " +
                    "IMAGES (ID INTEGER PRIMARY KEY, PATH VARCHAR(1000), " +
                    "RID INTEGER)";
                SqliteCommand createImageDb = new SqliteCommand(createImageDbText, db);
                createImageDb.ExecuteReader();

                String createIngredientDbText = "CREATE TABLE IF NOT " +
                    "EXISTS INGREDIENTS (ID INTEGER PRIMARY KEY, " +
                    "QUANTITY DOUBLE, UOM VARCHAR(50), NAME VARCHAR(100)," +
                    "RID INTEGER)";
                SqliteCommand createIngredientDb = new SqliteCommand(createIngredientDbText, db);
                createIngredientDb.ExecuteReader();

                String createStepsDbText = "CREATE TABLE IF NOT " +
                    "EXISTS STEPS (ID INTEGER PRIMARY KEY, STEPORDER INTEGER, " +
                    "DESCRIPTION VARCHAR(1000), RID INTEGER)";
                SqliteCommand createStepsDb = new SqliteCommand(createStepsDbText, db);
                createStepsDb.ExecuteReader();

            }
            catch (SqliteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of all saved recipes in the database. These
        /// recipes will have all the information that has been saved
        /// that is associated when a recipe.
        /// </summary>
        /// <returns>
        /// a list of the recipes saved in the database
        /// </returns>
        public static ObservableCollection<Recipe> getSavedRecipes()
        {
            ObservableCollection<Recipe> savedRecipes = new ObservableCollection<Recipe>();
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand("SELECT * from RECIPES", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long id = query.GetInt64(0);
                String name = query.GetString(1);
                double rating = query.GetDouble(2);
                Recipe savedRecipe = new Recipe(name, id, rating);
                savedRecipes.Add(savedRecipe);
            }

            db.Close();
            return savedRecipes;
        }

        /// <summary>
        /// Gets the ingredients related to the given recipe id
        /// </summary>
        /// <param name="recipeId">
        /// the ID for the recipe to get ingredients for
        /// </param>
        /// <returns>
        /// a list of ingredients for the related recipe
        /// </returns>
        public static ObservableCollection<RecipeIngredient> getIngredients(long recipeId)
        {
            ObservableCollection<RecipeIngredient> savedIngredients = new ObservableCollection<RecipeIngredient>();
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand("SELECT * from INGREDIENTS WHERE RID = " + recipeId, db);
            
            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long id = query.GetInt64(0);
                double quantity = query.GetDouble(1);
                String unitOfMeasure = query.GetString(2);
                String ingredientName = query.GetString(3);
                long rid = query.GetInt64(4);
                RecipeIngredient savedIngredient = new RecipeIngredient(id, quantity, unitOfMeasure, ingredientName);
                savedIngredient.setRecipeId(rid);
                savedIngredients.Add(savedIngredient);
            }

            db.Close();
            return savedIngredients;
        }

        /// <summary>
        /// Gets the maximum ID value from the given table.
        /// </summary>
        /// <param name="tableName">
        /// The table to get the max ID value from
        /// </param>
        /// <returns>
        /// The maximum ID in the given table
        /// </returns>
        public static long getMaxId(String tableName)
        {
            long max = 0;
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            db.Open();

            String sqlStatement = "SELECT ID from " + tableName;
            SqliteCommand selectCommand = new SqliteCommand(sqlStatement, db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long value = query.GetInt64(0);
                if (value > max)
                {
                    max = value;
                }
            }

            db.Close();
            return max;
        }

        /// <summary>
        /// Adds the given recipe to the database
        /// </summary>
        /// <param name="newRecipe">
        /// the new recipe to add to the database
        /// </param>
        public static void addRecipe(Recipe newRecipe)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO RECIPES (ID, NAME, RATING) VALUES (@ID, @Name, @Rating);";
            insertCommand.Parameters.AddWithValue("@ID", newRecipe.ID);
            insertCommand.Parameters.AddWithValue("@Name", newRecipe.Name);
            insertCommand.Parameters.AddWithValue("@Rating", newRecipe.Rating);

            insertCommand.ExecuteReader();

            db.Close();
        }

        /// <summary>
        /// Adds the given ingredient to the database
        /// </summary>
        /// <param name="newIngredient">
        /// the new ingredient to add to the database
        /// </param>
        public static void addIngredient(RecipeIngredient newIngredient)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO INGREDIENTS " +
                "(ID, QUANTITY, UOM, NAME, RID) VALUES " +
                "(@ID, @Quantity, @UOM, @Name, @RecipeID)";
            insertCommand.Parameters.AddWithValue("@ID", newIngredient.ID);
            insertCommand.Parameters.AddWithValue("@Quantity", newIngredient.Quantity);
            insertCommand.Parameters.AddWithValue("@UOM", newIngredient.UnitOfMeasure);
            insertCommand.Parameters.AddWithValue("@Name", newIngredient.IngredientName);
            insertCommand.Parameters.AddWithValue("@RecipeID", newIngredient.RecipeID);

            insertCommand.ExecuteReader();

            db.Close();
        }

        /// <summary>
        /// Edits the given recipe in the database. This will cascade
        /// to any related data. 
        /// </summary>
        /// <param name="updatedRecipe">
        /// The recipe that will be updated
        /// </param>
        public static void updateRecipe(Recipe updatedRecipe)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand updateCommand = new SqliteCommand();
            updateCommand.Connection = db;

            updateCommand.CommandText = "UPDATE RECIPES SET NAME = @Name, RATING = @Rating WHERE ID = @ID";
            updateCommand.Parameters.AddWithValue("@ID", updatedRecipe.ID);
            updateCommand.Parameters.AddWithValue("@Name", updatedRecipe.Name);
            updateCommand.Parameters.AddWithValue("@Rating", updatedRecipe.Rating);

            updateCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Deleted the given recipe from the database. This will
        /// cascade and remove all related data as well.
        /// </summary>
        /// <param name="deletingRecipe">
        /// the recipe that will be deleted
        /// </param>
        public static void deleteRecipe(Recipe deletingRecipe)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = db;

            deleteCommand.CommandText = "DELETE FROM RECIPES WHERE ID = @ID";
            deleteCommand.Parameters.AddWithValue("@ID", deletingRecipe.ID);

            deleteCommand.ExecuteNonQuery();

            db.Close();
        }
    }
}