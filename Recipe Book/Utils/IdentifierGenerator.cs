using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Utils
{
    /// <summary>
    /// Class IdentifierGenerator represents an ID generation class
    /// that should be used to generate IDs for the different  entities
    /// in this application. This includes images, recipes, ingredients,
    /// and steps. The client is responsible for making sure the seed
    /// ID is a valid starting ID based on the table it has been created for.
    /// </summary>
    public class IdentifierGenerator
    {
        /// <summary>
        /// The default step amount for an ID generator which is always 1.
        /// </summary>
        public const long DEFAULT_STEP = 1;

        /// <summary>
        /// The default ID for an ID generator
        /// </summary>
        public const long DEFAULT_ID = 0;

        private long startingId; // the id used to start this generator
        private long nextId; // the next id in the sequence
        private String tableName; // the name of the table this generator is for
        private long previousId; // the last ID generated
        private long step; // the increment used to increase the id

        /// <summary>
        /// A default constructor that creates a generator for the
        /// given table with the given starting ID value.
        /// </summary>
        /// <param name="tableName">
        /// The table for this generator
        /// </param>
        /// <param name="startingId">
        /// the ID to start with. This ID is assumed to be the next
        /// valid ID and will be returned when getId is called.
        /// </param>
        public IdentifierGenerator(String tableName, long startingId)
        {
            this.tableName = tableName;
            this.startingId = startingId;
            this.nextId = startingId;
            this.step = DEFAULT_STEP;
        }

        /// <summary>
        /// An alternative constructor that creates a generator using
        /// the additional step amount.
        /// </summary>
        /// <param name="tableName">
        /// The table for this generator
        /// </param>
        /// <param name="startingId">
        /// the ID to start with. This ID is assumed to be the next
        /// valid ID and will be returned when getId is called.
        /// </param>
        /// <param name="step">
        /// The amount to increment the ID by
        /// </param>
        public IdentifierGenerator(String tableName, long startingId, long step)
        {
            this.tableName = tableName;
            this.startingId = startingId;
            this.step = step;
            this.nextId = startingId;
        }

        /// <summary>
        /// Gets the next available ID
        /// </summary>
        /// <returns>
        /// The ID that is ready to be used
        /// </returns>
        public long getId()
        {
            increment();
            return previousId;
        }

        /// <summary>
        /// Increments the ID by the step.
        /// </summary>
        public void increment()
        {
            previousId = nextId;
            nextId += step;
        }

        /// <summary>
        /// Gets the name of the table this generator is for
        /// </summary>
        /// <returns>
        /// The name of the table using this generator
        /// </returns>
        public String getTableName()
        {
            return tableName;
        }

        /// <summary>
        /// Resets the IdentifierGenerator to a "factory default". This
        /// should only be done if the user's db has been truncated.
        /// </summary>
        public void reset()
        {
            this.startingId = DEFAULT_ID;
            this.nextId = startingId;
            this.step = DEFAULT_STEP;
        }
    }
}
