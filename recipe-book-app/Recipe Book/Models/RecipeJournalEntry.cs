using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Recipe_Book.Models
{
    public class RecipeJournalEntry
    {
        public const String TABLE_NAME = "JOURNAL_ENTRIES";

        private const String ID_KEY = "id";
        private const String RECIPE_RATING_KEY = "rating";
        private const String ENTRY_NOTES = "notes";
        private const String ENTRY_DATE_KEY = "entryDate";
        private const String RECIPE_ID_KEY = "recieId";

        private DateTimeOffset entryDate;
        private double recipeRating;
        private String entryNotes;
        private long recipeId;
        private long id;

        /// <summary>
        /// Gets the ID for this journal entry.
        /// </summary>
        public long ID
        {
            get
            {
                return getId();
            }
        }

        /// <summary>
        /// Gets or sets the ID of the recipe associated with this
        /// journal entry.
        /// </summary>
        public long RecipeID
        {
            get
            {
                return getRecipeId();
            }
            set
            {
                setRecipeId(value);
            }
        }

        /// <summary>
        /// Gets or sets the date for which this journal entry is
        /// meant.
        /// </summary>
        public DateTimeOffset EntryDate
        {
            get
            {
                return getDate();
            }
            set
            {
                setEntryDate(value);
            }
        }

        /// <summary>
        /// Gets or sets the rating that was given to the instance of
        /// the recipe that was made with this journal entry.
        /// </summary>
        public double Rating
        {
            get
            {
                return getRecipeRating();
            }
            set
            {
                setRating(value);
            }
        }

        /// <summary>
        /// Gets or sets the notes that are associated with this
        /// journal entry.
        /// </summary>
        public String EntryNotes
        {
            get
            {
                return getEntryNotes();
            }
            set
            {
                setEntryNotes(value);
            }
        }

        /// <summary>
        /// Creates a new and blank RecipeJournalEntry for the given
        /// ID.
        /// </summary>
        /// <param name="id">
        /// The ID for this journal entry.
        /// </param>
        public RecipeJournalEntry(long id)
        {
            this.id = id;
        }

        /// <summary>
        /// Sets the date of the journal entry to the given value.
        /// </summary>
        /// <param name="newDate">
        /// The new date for the joyrnal entry.
        /// </param>
        public void setEntryDate(DateTimeOffset newDate)
        {
            entryDate = newDate;
        }

        /// <summary>
        /// Sets the rating for the recipe that is associated with this
        /// journal entry.
        /// </summary>
        /// <param name="newRating">
        /// The new rating for this journal entry.
        /// </param>
        public void setRating(double newRating)
        {
            recipeRating = newRating;
        }

        /// <summary>
        /// Sets the notes for this journal entry.
        /// </summary>
        /// <param name="newNotes">
        /// The new notes for this journal entry.
        /// </param>
        public void setEntryNotes(String newNotes)
        {
            entryNotes = newNotes;
        }

        /// <summary>
        /// Sets the id of the associated recipe for this journal
        /// entry.
        /// </summary>
        /// <param name="newId">
        /// The new ID for the associated recipe.
        /// </param>
        public void setRecipeId(long newId)
        {
            recipeId = newId;
        }

        /// <summary>
        /// Gets the date for this journal entry.
        /// </summary>
        /// <returns>
        /// The date for this journal entry.
        /// </returns>
        public DateTimeOffset getDate()
        {
            return entryDate;
        }

        /// <summary>
        /// Gets the rating for this journal entry.
        /// </summary>
        /// <returns>
        /// The rating for this journal entry.
        /// </returns>
        public double getRecipeRating()
        {
            return recipeRating;
        }

        /// <summary>
        /// Gets the notes associated with this journal entry.
        /// </summary>
        /// <returns>
        /// The notes associated with this journal entry.
        /// </returns>
        public String getEntryNotes()
        {
            return entryNotes;
        }

        /// <summary>
        /// Gets the recipe that this journal entry is for.
        /// </summary>
        /// <returns>
        /// The ID for the recipe this journal entry is for.
        /// </returns>
        public long getRecipeId()
        {
            return recipeId;
        }

        /// <summary>
        /// Gets the ID for this journal entry.
        /// </summary>
        /// <returns>
        /// The ID for this journal entry.
        /// </returns>
        public long getId()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            RecipeJournalEntry other = (RecipeJournalEntry)obj;
            return base.Equals(other) && other.getId() == this.id;
        }

        /// <summary>
        /// Converts this journal entry into a JSON object.
        /// </summary>
        /// <returns>
        /// A JsonObject that represents this journal entry.
        /// </returns>
        public JsonObject toJsonObject()
        {
            JsonObject entryObject = new JsonObject();
            entryObject.SetNamedValue(ID_KEY, JsonValue.CreateNumberValue(id));
            entryObject.SetNamedValue(RECIPE_RATING_KEY, JsonValue.CreateNumberValue(recipeRating));
            entryObject.SetNamedValue(ENTRY_NOTES, JsonValue.CreateStringValue(entryNotes));
            entryObject.SetNamedValue(ENTRY_DATE_KEY, JsonValue.CreateStringValue(entryDate.ToString()));
            entryObject.SetNamedValue(RECIPE_ID_KEY, JsonValue.CreateNumberValue(recipeId));

            return entryObject;
        }
    }
}
