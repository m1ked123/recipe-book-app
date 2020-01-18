using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Recipe_Book.Models
{
    public class RecipeIngredient : INotifyPropertyChanged
    {
        public const String TABLE_NAME = "INGREDIENTS";

        private const String ID_KEY = "id";
        private const String INGREDIENT_NAME_KEY = "name";
        private const String QUANTITY_KEY = "quantity";
        private const String UOM_KEY = "uom";
        private const String RECIPE_ID_KEY = "recieId";
        

        private long id;
        private String quantity;
        private String unitOfMeasure;
        private String ingredientName;
        private long recipeId;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the quantity of this recipe ingredient.
        /// </summary>
        public String Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                setQuantity(value);
                RaisePropertyChanged("Quantity");
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
                RaisePropertyChanged("UnitOfMeasure");
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
                RaisePropertyChanged("IngredientName");
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
                RaisePropertyChanged("RecipeID");
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
        public RecipeIngredient(long id, String quantity, 
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
        public void setQuantity(String newQuantity)
        {
            if (newQuantity.Length != 0)
            {
                this.quantity = newQuantity;
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
            }
        }

        protected void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Serializes this ingredient to JSON object.
        /// </summary>
        /// <returns>
        /// The string representation of hte JSON object equivalent of
        /// this ingredient.
        /// </returns>
        public String serialize()
        {
            JsonObject ingredientObject = new JsonObject();
            ingredientObject.SetNamedValue(ID_KEY, JsonValue.CreateNumberValue(id));
            ingredientObject.SetNamedValue(INGREDIENT_NAME_KEY, JsonValue.CreateStringValue(ingredientName));
            ingredientObject.SetNamedValue(QUANTITY_KEY, JsonValue.CreateStringValue(quantity));
            ingredientObject.SetNamedValue(UOM_KEY, JsonValue.CreateStringValue(unitOfMeasure));
            ingredientObject.SetNamedValue(RECIPE_ID_KEY, JsonValue.CreateNumberValue(recipeId));

            return ingredientObject.Stringify();
        }
    }
}
