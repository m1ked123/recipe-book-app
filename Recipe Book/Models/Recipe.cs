using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Book.Models
{
    class Recipe:INotifyPropertyChanged
    {
        private String name;
        private long id;
        private double rating;
        private String lastMade;

        public event PropertyChangedEventHandler PropertyChanged;

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public double Rating
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
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rating"));
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

        public Recipe(String name, long id, double rating) : this(name, id, 0, "Never") {}

        public Recipe(String name, long id, double rating, String lastMade)
        {
            this.name = name;
            this.id = id;
            this.rating = rating;
            this.lastMade = lastMade;
        }
    }
}
