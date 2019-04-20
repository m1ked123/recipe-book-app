using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Recipe_Book.Models
{
    public class Recipe : INotifyPropertyChanged
    {
        private String name;
        private long id;
        private double rating;
        private String lastMade;
        private ObservableCollection<RecipeImage> recipeImages;
        private ObservableCollection<RecipeIngredient> recipeIngredients;

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
                // PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
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
                    // PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rating"));
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

        public ObservableCollection<RecipeImage> RecipeImages
        {
            get
            {
                return this.recipeImages;
            }
            set
            {
                this.recipeImages = value;
            }
        }

        public ObservableCollection<RecipeIngredient> RecipeIngredients
        {
            get
            {
                return this.recipeIngredients;
            }
        }

        public Recipe() : this("New Recipe") {}

        public Recipe(String name) : this(name, -1) {}

        public Recipe(String name, long id) : this(name, id, 0) {}

        public Recipe(String name, long id, double rating) : this(name, id, 0, "Never") {}

        public Recipe(String name, long id, double rating, String lastMade) : this(name, id, rating, lastMade, new ObservableCollection<RecipeImage>()) { }

        public Recipe(String name, long id, double rating, String lastMade, ObservableCollection<RecipeImage> images) : this(name, id, rating, lastMade, new ObservableCollection<RecipeImage>(), new ObservableCollection<RecipeIngredient>()) { }

        public Recipe(String name, long id, double rating, String lastMade, ObservableCollection<RecipeImage> images, ObservableCollection<RecipeIngredient> ingredients)
        {
            this.name = name;
            this.id = id;
            this.rating = rating;
            this.lastMade = lastMade;
            this.recipeImages = images;
            this.recipeIngredients = ingredients;
        }

        public void addImage(String imagePath)
        {
            this.recipeImages.Add(new RecipeImage(imagePath));
        }

        public void addImage(RecipeImage newImage)
        {
            this.recipeImages.Add(newImage);
        }

        public void setImages(ObservableCollection<RecipeImage> newImages)
        {
            this.recipeImages = newImages;
        }
    }
}
