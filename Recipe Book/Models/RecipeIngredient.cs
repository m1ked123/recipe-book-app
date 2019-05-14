using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Models
{
    public class RecipeIngredient : INotifyPropertyChanged
    {
        public const String TABLE_NAME = "INGREDIENTS";

        private long id;
        private double quantity;
        private String unitOfMeasure;
        private String ingredientName;
        private long recipeId;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the quantity of this recipe ingredient.
        /// </summary>
        public double Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                setQuantity(value);
            }
        }

        /// <summary>
        /// Gets or sets the unit of measure used for the recipe
        /// ingredient
        /// </summary>
        public String UnitOfMeasure
        {
            get
            {
                return this.unitOfMeasure;
            }
            set
            {
                setUnitOfMeasure(value);
            }
        }

        /// <summary>
        /// Gets or sets the name of this ingredient
        /// </summary>
        public String IngredientName
        {
            get
            {
                return this.ingredientName;
            }
            set
            {
                setIngredientName(value);
            }
        }

        /// <summary>
        /// Gets the ID of this ingredient
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
        /// Gets the ID of the recipe for this ingredient
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
        /// Constructs a new ingredient with the given ID, related
        /// to the recipe with the given ID, in the given quantity,
        /// with the given unit of measure, and the given name.
        /// </summary>
        /// <param name="id">
        /// the ID for this ingredient
        /// </param>
        /// <param name="recipeId">
        /// the id of the recipe this ingredient is related to
        /// </param>
        /// <param name="quantity">
        /// the amount of this ingredient in the related recipe
        /// </param>
        /// <param name="unitOfMeasure">
        /// the unit of measure for the quantity of this ingredient
        /// </param>
        /// <param name="ingredientName">
        /// the name of this ingredient
        /// </param>
        public RecipeIngredient(long id, double quantity, 
            String unitOfMeasure, String ingredientName)
        {
            this.quantity = quantity;
            this.unitOfMeasure = unitOfMeasure;
            this.ingredientName = ingredientName;
            this.id = id;
            this.recipeId = -1;
        }

        /// <summary>
        /// Gets the name of this ingredient
        /// </summary>
        /// <returns>
        /// the name of this ingredient
        /// </returns>
        public String getName()
        {
            return this.ingredientName;
        }

        /// <summary>
        /// Gets the ID of this ingredient
        /// </summary>
        /// <returns>
        /// the ID of this ingredient
        /// </returns>
        public long getId()
        {
            return this.id;
        }

        /// <summary>
        /// Gets the ID of the recipe that  this ingredient is for
        /// </summary>
        /// <returns>
        /// the id of the related recipe
        /// </returns>
        public long getRecipeId()
        {
            return this.recipeId;
        }

        /// <summary>
        /// Sets the related recipe to the given id
        /// </summary>
        /// <param name="recipeId">
        /// the id of the recipe this ingredient is related to
        /// </param>
        public void setRecipeId(long recipeId)
        {
            this.recipeId = recipeId;
        }

        /// <summary>
        /// Sets the quantity of the ingredient to the given amount if
        /// it's positive.
        /// </summary>
        /// <param name="newQuantity">
        /// the new positive quantity of the ingredient
        /// </param>
        public void setQuantity(double newQuantity)
        {
            if (newQuantity > 0)
            {
                this.quantity = newQuantity;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IngredientQuantity"));
            }
        }

        /// <summary>
        /// Sets the unit of measure to the given non-empty value
        /// </summary>
        /// <param name="newUOM">
        /// the new unit of measure
        /// </param>
        public void setUnitOfMeasure(String newUOM)
        {
            if (newUOM.Length > 0)
            {
                this.unitOfMeasure = newUOM;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IngredientUOM"));
            }
        }

        /// <summary>
        /// Sets the name of the ingredient to the given new name
        /// </summary>
        /// <param name="newIngredientName">
        /// the new name for the ingredient
        /// </param>
        public void setIngredientName(String newIngredientName)
        {
            if (newIngredientName.Length > 0)
            {
                this.ingredientName = newIngredientName;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IngredientName"));
            }
        }
    }
}
