using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Models
{
    class Recipe
    {
        private String name;
        private short rating;
        private String lastMade;

        public Recipe()
        {
            this.name = "";
            this.rating = 0;
            this.lastMade = "";
        }
    }
}
