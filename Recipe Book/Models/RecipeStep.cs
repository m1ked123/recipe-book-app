using System;

namespace Recipe_Book.Models
{
    /// <summary>
    /// Class RecipeStep represents a step for making a recipe in the
    /// recipe book.
    /// </summary>
    public class RecipeStep
    {
        public const String TABLE_NAME = "STEPS";

        private String stepDescription;
        private int order;
        private long id;
        private long recipeId;

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
                setDescription(value);
            }
        }

        /// <summary>
        /// Gets or sets the order of this step in the recipe
        /// </summary>
        public int Order
        {
            get
            {
                return getOrder();
            }
            set
            {
                setOrder(value);
            }
        }

        /// <summary>
        /// Gets the ID of this recipe step
        /// </summary>
        public long ID
        {
            get
            {
                return getId();
            }
            private set { }
        }

        /// <summary>
        /// Gets or sets the ID of the recipe that this step is for
        /// </summary>
        public long RecipeID
        {
            get {
                return getRecipeId();
            }
            set
            {
                setRecipeId(value);
            }
        }

        /// <summary>
        /// Creates a new step for the recipe giving it the given ID,
        /// order, and description.
        /// </summary>
        /// <param name="id">
        /// the ID for this recipe step
        /// </param>
        /// <param name="order">
        /// the order of this stamp in this recipe
        /// </param>
        /// <param name="description">
        /// the description for this recipe step
        /// </param>
        public RecipeStep(long id, int order, String description)
        {
            this.id = id;
            this.order = order;
            this.stepDescription = description;
            this.recipeId = -1;
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
        /// Gets the order of this recipe step in the recipe
        /// </summary>
        /// <returns>
        /// the order of the step in the recipe
        /// </returns>
        public int getOrder()
        {
            return this.order;
        }

        /// <summary>
        /// Gets the ID of this recipe step
        /// </summary>
        /// <returns>
        /// the ID of this recipe step
        /// </returns>
        public long getId()
        {
            return this.id;
        }

        /// <summary>
        /// Gets the recipe ID for this recipe step
        /// </summary>
        /// <returns>
        /// the id for the recipe associated with this step
        /// </returns>
        public long getRecipeId()
        {
            return this.recipeId;
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

        /// <summary>
        /// Sets the order of this recipe step to the given number
        /// </summary>
        /// <param name="newOrder">
        /// the new order number for this step
        /// </param>
        public void setOrder(int newOrder)
        {
            this.order = newOrder;
        }

        /// <summary>
        /// Sets the recipe ID to the given number. This number must
        /// be positive.
        /// </summary>
        /// <param name="recipeId">
        /// the new id for the recipe to associate this step to.
        /// </param>
        public void setRecipeId(long recipeId)
        {
            if (recipeId >= 0)
            {
                this.recipeId = recipeId;
            }
        }
    }
}
