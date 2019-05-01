using System;

namespace Recipe_Book.Models
{
    /// <summary>
    /// Class RecipeStep represents a step for making a recipe in the
    /// recipe book.
    /// </summary>
    public class RecipeStep
    {
        private String stepDescription;

        /// <summary>
        /// The description of this recipe step.
        /// </summary>
        public String StepDescription
        {
            get
            {
                return getDescription();
            }
            set
            {
                this.setDescription(value);
            }
        }

        /// <summary>
        /// The default constructor for a recipe step. It initializes
        /// a default step that has a default test string. This is
        /// not the preferred constructor to use, but is available for
        /// convenience.
        /// </summary>
        public RecipeStep() : this("Recipe step description") { }

        /// <summary>
        /// Creates a new recipe steop with the given description.
        /// </summary>
        /// <param name="stepDescription">
        /// the description of this recipe step
        /// </param>
        public RecipeStep(String stepDescription)
        {
            this.stepDescription = stepDescription;
        }

        /// <summary>
        /// Gets the description for this recipe step.
        /// </summary>
        /// <returns>the description for this recipe sept</returns>
        public String getDescription()
        {
            return this.stepDescription;
        }

        /// <summary>
        /// Sets the description of this step to the provided 
        /// desctiption. This will only occur if the new description
        /// isn't an empty string.
        /// </summary>
        /// <param name="newDescription">
        /// the new description for this recipe step. It must not be
        /// an empty string.
        /// </param>
        public void setDescription(String newDescription)
        {
            if (newDescription.Length > 0)
            {
                this.stepDescription = newDescription;
            }
        }
    }
}
