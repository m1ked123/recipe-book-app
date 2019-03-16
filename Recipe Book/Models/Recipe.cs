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
        private long id;
        private short rating;
        private String lastMade;

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public short Rating
        {
            get
            {
                return this.rating;
            }
            set
            {
                if (value >= 0 && value < 6)
                {
                    this.rating = value;
                }
            }
        }

        public String LastMade
        {
            get
            {
                return this.lastMade;
            }
            set
            {
                this.lastMade = value;
            }
        }

        public long ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public Recipe() : this("New Recipe") {}

        public Recipe(String name) : this(name, -1) {}

        public Recipe(String name, long id) : this(name, id, 0) {}

        public Recipe(String name, long id, short rating) : this(name, id, 0, "Never") {}

        public Recipe(String name, long id, short rating, String lastMade)
        {
            this.name = name;
            this.id = id;
            this.rating = rating;
            this.lastMade = lastMade;
        }
    }
}
