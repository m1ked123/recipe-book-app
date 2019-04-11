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
        private float quantity;
        private String unitOfMeasure;
        private String ingredientName;

        public event PropertyChangedEventHandler PropertyChanged;

        public float Quantity
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

        public RecipeIngredient(float quantity, String unitOfMeasure, String ingredientName)
        {
            this.quantity = quantity;
            this.unitOfMeasure = unitOfMeasure;
            this.ingredientName = ingredientName;
        }

        public void setQuantity(float newQuantity)
        {
            if (newQuantity > 0)
            {
                this.quantity = newQuantity;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IngredientQuantity"));
            }
        }

        public void setUnitOfMeasure(String newUOM)
        {
            if (newUOM.Length > 0)
            {
                this.unitOfMeasure = newUOM;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IngredientUOM"));
            }
        }

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
