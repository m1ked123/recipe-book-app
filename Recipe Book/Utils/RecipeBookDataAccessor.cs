using Microsoft.Data.Sqlite;
using Recipe_Book.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Recipe_Book.Utils
{
    /// <summary>
    /// Class RecipeBookDataAccessor contains several static methods
    /// that are used to persist data in the application database.
    /// </summary>
    public class RecipeBookDataAccessor
    {
        /// <summary>
        /// Initializes the database that will be used to store recipe
        /// book items.
        /// </summary>
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
                createImageDb.ExecuteNonQuery();

                String createIngredientDbText = "CREATE TABLE IF NOT " +
                    "EXISTS INGREDIENTS (ID INTEGER PRIMARY KEY, " +
                    "QUANTITY VARCHAR(5), UOM VARCHAR(50), NAME VARCHAR(100)," +
                    "RID INTEGER)";
                SqliteCommand createIngredientDb = new SqliteCommand(createIngredientDbText, db);
                createIngredientDb.ExecuteNonQuery();

                String createStepsDbText = "CREATE TABLE IF NOT " +
                    "EXISTS STEPS (ID INTEGER PRIMARY KEY, STEPORDER INTEGER, " +
                    "DESCRIPTION VARCHAR(1000), RID INTEGER)";
                SqliteCommand createStepsDb = new SqliteCommand(createStepsDbText, db);
                createStepsDb.ExecuteNonQuery();

                String createJournalEntryDbText = "CREATE TABLE IF" +
                    " NOT EXISTS JOURNAL_ENTRIES (ID INTEGER" +
                    " PRIMARY KEY, RID INTEGER, ENTRYNOTES VARCHAR(5000)," +
                    " RATING DOUBLE, ENTRYDATE DATETIME)";
                SqliteCommand createJournalDb = new SqliteCommand(createJournalEntryDbText, db);
                createJournalDb.ExecuteNonQuery();

                db.Close();

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
                Recipe savedRecipe = new Recipe(id, name, rating);
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

            SqliteCommand selectCommand = new SqliteCommand("SELECT * FROM INGREDIENTS WHERE RID = " + recipeId, db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long id = query.GetInt64(0);
                String quantity = query.GetString(1);
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
        /// Gets the list of steps related to the recipe with the given ID
        /// </summary>
        /// <param name="recipeId">
        /// the ID for the recipe to get steps for
        /// </param>
        /// <returns>
        /// a list of the steps for the related recipe
        /// </returns>
        public static ObservableCollection<RecipeStep> getSteps(long recipeId)
        {
            ObservableCollection<RecipeStep> savedSteps = new ObservableCollection<RecipeStep>();
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand("SELECT * from STEPS WHERE RID = " + recipeId +
                " ORDER BY STEPORDER ASC", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long id = query.GetInt64(0);
                int order = query.GetInt32(1);
                String description = query.GetString(2);
                long rid = query.GetInt64(3);
                RecipeStep savedStep = new RecipeStep(id, order, description);
                savedStep.setRecipeId(rid);
                savedSteps.Add(savedStep);
            }

            db.Close();
            return savedSteps;
        }

        /// <summary>
        /// Gets a list of all the images that are associated to the
        /// given recipe ID.
        /// </summary>
        /// <param name="recipeId">
        /// the ID of the recipe for which images will be retrieved.
        /// </param>
        /// <returns>
        /// an observable collection of images associated to the given
        /// recipe.
        /// </returns>
        public static ObservableCollection<RecipeImage> getImages(long recipeId)
        {
            ObservableCollection<RecipeImage> savedImages = new ObservableCollection<RecipeImage>();
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand("SELECT * from IMAGES WHERE RID = " + recipeId, db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long id = query.GetInt64(0);
                String path = query.GetString(1);
                long rid = query.GetInt64(2);
                RecipeImage savedImage = new RecipeImage(id, path);
                savedImage.RecipeID = rid;
                savedImages.Add(savedImage);
            }

            db.Close();
            return savedImages;
        }

        public static ObservableCollection<RecipeJournalEntry> getJournalEntries(long recipeId)
        {
            ObservableCollection<RecipeJournalEntry> savedjournalEntries = new ObservableCollection<RecipeJournalEntry>();
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand("SELECT * from JOURNAL_ENTRIES WHERE RID = @RecipeID");
            selectCommand.Connection = db;
            selectCommand.Parameters.AddWithValue("@RecipeID", recipeId);
            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                long id = query.GetInt64(0);
                String entryNotes = query.GetString(2);
                double rating = query.GetDouble(3);
                DateTime entryDate = query.GetDateTime(4);
                RecipeJournalEntry savedEntry = new RecipeJournalEntry(id);
                savedEntry.RecipeID = recipeId;
                savedEntry.EntryDate = entryDate;
                savedEntry.EntryNotes = entryNotes;
                savedEntry.Rating = rating;
                savedjournalEntries.Add(savedEntry);
            }

            db.Close();
            Debug.WriteLine("Saved Entries: " + savedjournalEntries.Count);
            return savedjournalEntries;
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

        public static void updateIngredient(RecipeIngredient updatedIngredient)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand updateCommand = new SqliteCommand();
            updateCommand.Connection = db;

            updateCommand.CommandText = "UPDATE INGREDIENTS SET QUANTITY = @Quantity," +
                " UOM = @UOM, NAME = @Name WHERE ID = @ID";
            updateCommand.Parameters.AddWithValue("@ID", updatedIngredient.ID);
            updateCommand.Parameters.AddWithValue("@Quantity", updatedIngredient.Quantity);
            updateCommand.Parameters.AddWithValue("@UOM", updatedIngredient.UnitOfMeasure);
            updateCommand.Parameters.AddWithValue("@Name", updatedIngredient.IngredientName);

            updateCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Adds the given step to the database
        /// </summary>
        /// <param name="newStep">
        /// the new step to add
        /// </param>
        public static void addStep(RecipeStep newStep)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO STEPS " +
                "(ID, STEPORDER, DESCRIPTION, RID) VALUES " +
                "(@ID, @Order, @Description, @RecipeID)";
            insertCommand.Parameters.AddWithValue("@ID", newStep.ID);
            insertCommand.Parameters.AddWithValue("@Order", newStep.Order);
            insertCommand.Parameters.AddWithValue("@Description", newStep.StepDescription);
            insertCommand.Parameters.AddWithValue("@RecipeID", newStep.RecipeID);

            insertCommand.ExecuteReader();

            db.Close();
        }

        /// <summary>
        /// Updates the recipe step with the given updated information.
        /// </summary>
        /// <param name="updatedStep">
        /// The updated step
        /// </param>
        public static void updateStep(RecipeStep updatedStep)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand updateCommand = new SqliteCommand();
            updateCommand.Connection = db;

            updateCommand.CommandText = "UPDATE STEPS SET STEPORDER = @Order," +
                " DESCRIPTION = @Description WHERE ID = @ID";
            updateCommand.Parameters.AddWithValue("@Order", updatedStep.Order);
            updateCommand.Parameters.AddWithValue("@Description", updatedStep.StepDescription);
            updateCommand.Parameters.AddWithValue("@ID", updatedStep.ID);

            updateCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Adds the given recipe image to the database.
        /// </summary>
        /// <param name="newImage">
        /// the new image to add
        /// </param>
        public static void addImage(RecipeImage newImage)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO IMAGES " +
                "(ID, PATH, RID) VALUES " +
                "(@ID, @Path, @RecipeID)";
            insertCommand.Parameters.AddWithValue("@ID", newImage.ID);
            insertCommand.Parameters.AddWithValue("@Path", newImage.ImagePath);
            insertCommand.Parameters.AddWithValue("@RecipeID", newImage.RecipeID);

            insertCommand.ExecuteReader();

            db.Close();
        }

        public static void addJournalEntry(RecipeJournalEntry newEntry)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            insertCommand.CommandText = "INSERT INTO JOURNAL_ENTRIES " +
                "(ID, RID, ENTRYNOTES, RATING, ENTRYDATE) VALUES " +
                "(@ID, @RecipeID, @EntryNotes, @Rating, @EntryDate)";
            insertCommand.Parameters.AddWithValue("@ID", newEntry.ID);
            insertCommand.Parameters.AddWithValue("@RecipeID", newEntry.RecipeID);
            insertCommand.Parameters.AddWithValue("@EntryNotes", newEntry.EntryNotes);
            insertCommand.Parameters.AddWithValue("@Rating", newEntry.Rating);
            insertCommand.Parameters.AddWithValue("@EntryDate", newEntry.EntryDate);

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

            SqliteCommand deleteRecipeCommand = new SqliteCommand();
            SqliteCommand deleteIngredientCommand = new SqliteCommand();
            SqliteCommand deleteImagesCommand = new SqliteCommand();
            SqliteCommand deleteStepsCommand = new SqliteCommand();

            deleteRecipeCommand.Connection = db;
            deleteIngredientCommand.Connection = db;
            deleteImagesCommand.Connection = db;
            deleteStepsCommand.Connection = db;

            deleteRecipeCommand.CommandText = "DELETE FROM RECIPES WHERE ID = @ID";
            deleteIngredientCommand.CommandText = "DELETE FROM INGREDIENTS WHERE RID = @ID";
            deleteImagesCommand.CommandText = "DELETE FROM IMAGES WHERE RID = @ID";
            deleteStepsCommand.CommandText = "DELETE FROM STEPS WHERE RID = @ID";

            deleteRecipeCommand.Parameters.AddWithValue("@ID", deletingRecipe.ID);
            deleteIngredientCommand.Parameters.AddWithValue("@ID", deletingRecipe.ID);
            deleteImagesCommand.Parameters.AddWithValue("@ID", deletingRecipe.ID);
            deleteStepsCommand.Parameters.AddWithValue("@ID", deletingRecipe.ID);

            deleteRecipeCommand.ExecuteNonQuery();
            deleteStepsCommand.ExecuteNonQuery();
            deleteIngredientCommand.ExecuteNonQuery();
            deleteImagesCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Removes the given RecipeIngredient from the database
        /// </summary>
        /// <param name="deletingIngredient">
        /// the ingredient to delete
        /// </param>
        public static void deleteIngredient(RecipeIngredient deletingIngredient)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = db;

            deleteCommand.CommandText = "DELETE FROM INGREDIENTS WHERE ID = @ID";
            deleteCommand.Parameters.AddWithValue("@ID", deletingIngredient.ID);

            deleteCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Deletes the given recipe step from the database.
        /// </summary>
        /// <param name="deletingStep">
        /// the recipe step to delete
        /// </param>
        public static void deleteStep(RecipeStep deletingStep)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = db;

            deleteCommand.CommandText = "DELETE FROM STEPS WHERE ID = @ID";
            deleteCommand.Parameters.AddWithValue("@ID", deletingStep.ID);

            deleteCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Deletes the given image from the database
        /// </summary>
        /// <param name="deletingImage">
        /// the image to delete
        /// </param>
        public static void deleteImage(RecipeImage deletingImage)
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = db;

            deleteCommand.CommandText = "DELETE FROM IMAGES WHERE ID = @ID";
            deleteCommand.Parameters.AddWithValue("@ID", deletingImage.ID);

            deleteCommand.ExecuteNonQuery();

            db.Close();
        }

        /// <summary>
        /// Empties the recipe list in the user's backend app DB.
        /// </summary>
        public static void emptyRecipteList()
        {
            SqliteConnection db = new SqliteConnection("Filename=RecipeBook.db");

            db.Open();

            SqliteCommand truncateRecipesCommand = new SqliteCommand();
            SqliteCommand truncateImagesCommand = new SqliteCommand();
            SqliteCommand truncateStepsCommand = new SqliteCommand();
            SqliteCommand truncateIngredientsCommand = new SqliteCommand();

            truncateRecipesCommand.Connection = db;
            truncateImagesCommand.Connection = db;
            truncateStepsCommand.Connection = db;
            truncateIngredientsCommand.Connection = db;

            truncateRecipesCommand.CommandText = "DELETE FROM RECIPES";
            truncateImagesCommand.CommandText = "DELETE FROM IMAGES";
            truncateStepsCommand.CommandText = "DELETE FROM STEPS";
            truncateIngredientsCommand.CommandText = "DELETE FROM INGREDIENTS";

            truncateRecipesCommand.ExecuteNonQuery();
            truncateImagesCommand.ExecuteNonQuery();
            truncateStepsCommand.ExecuteNonQuery();
            truncateIngredientsCommand.ExecuteNonQuery();

            db.Close();
        }
    }
}