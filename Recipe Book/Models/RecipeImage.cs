using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Recipe_Book.Models
{
    /// <summary>
    /// Class <code>RecipeImage</code> represents a relatively simple
    /// image object used by a <code>Recipe</code>.
    /// </summary>
    public class RecipeImage
    {
        private String imagePath;
        private BitmapImage internalImage;
        
        public String ImagePath
        {
            get
            {
                return this.imagePath;
            }
        }

        public BitmapImage InternalImage
        {
            get
            {
                return this.internalImage;
            }
        }

        public RecipeImage(String imagePath)
        {
            this.imagePath = imagePath;
        }

        public RecipeImage(BitmapImage internalImage)
        {
            this.internalImage = internalImage;
        }

        public Uri getImageUri()
        {
            return new Uri(this.imagePath);
        }

        public void setInternalImage (BitmapImage newImage)
        {
            this.internalImage = newImage;
        }
    }
}