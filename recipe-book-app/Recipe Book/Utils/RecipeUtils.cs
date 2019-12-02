using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Utils
{
    class RecipeUtils
    {
        // TODD: Make this function more robust
        public static List<String> getUnitsOfMeasure()
        {
            List<String> units = new List<string>();
            units.Add("Teaspoon");
            units.Add("Tablespoon");
            units.Add("Milligrams");
            units.Add("Grams");
            units.Add("Ounces");
            units.Add("Pounds");
            units.Add("Quarts");
            units.Add("Pints");
            units.Add("Cups");
            units.Add("Liters");
            units.Add("Milliliters");
            units.Add(" ");

            return units;
        }
    }
}