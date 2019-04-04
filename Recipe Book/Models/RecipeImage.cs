using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Recipe_Book.Models
{
    public class RecipeImage
    {
        private String imagePath;

        public String ImagePath
        {
            get
            {
                Debug.WriteLine(this.imagePath);
                return this.imagePath;
            }
        }

        public RecipeImage(String imagePath)
        {
            this.imagePath = imagePath;
        }

        public Uri getImageUri()
        {
            return new Uri(this.imagePath);
        }
    }
}