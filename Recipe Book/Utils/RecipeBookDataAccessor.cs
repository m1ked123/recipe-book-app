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
                    "QUANTITY DOUBLE, UOM VARCHAR(50), NAME VARCHAR(100))";
                SqliteCommand createIngredientDb = new SqliteCommand(createIngredientDbText, db);
                createIngredientDb.ExecuteReader();

                String createStepsDbText = "CREATE TABLE IF NOT " +
                    "EXISTS STEPS (ID INTEGER PRIMARY KEY, STEPORDER INTEGER, " +
                    "DESCRIPTION VARCHAR(1000))";
                SqliteCommand createStepsDb = new SqliteCommand(createStepsDbText, db);
                createStepsDb.ExecuteReader();

            }
            catch (SqliteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

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
        /// <param name="recipe">
        /// the new recipe to add to the database
        /// </param>
        public static void addRecipe(Recipe recipe)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO RECIPES (ID, NAME, RATING) VALUES (@ID, @Name, @Rating);";
            insertCommand.Parameters.AddWithValue("@ID", recipe.ID);
            insertCommand.Parameters.AddWithValue("@Name", recipe.Name);
            insertCommand.Parameters.AddWithValue("@Rating", recipe.Rating);

            insertCommand.ExecuteReader();

            db.Close();
        }
    }
}
