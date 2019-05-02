using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Utils
{
    public class RecipeBookDataAccessor
    {
        public static void InitializeDatabase()
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            try
            {
                db.Open();

                String createRecipeDbText = "CREATE TABLE IF NOT " +
                    "EXISTS RECIPES (ID INTEGER PRIMARY KEY, NAME " +
                    "VARCHAR(500), RATING INTEGER)";
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
    }
}
