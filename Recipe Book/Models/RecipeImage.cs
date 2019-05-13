using Recipe_Book.ViewModels;
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
        public const String TABLE_NAME = "IMAGES";
        private String imagePath;
        private BitmapImage internalImage;
        private long id;
        private long recipeId;

        /// <summary>
        /// Gets the ID of this recipe image
        /// </summary>
        public long ID
        {
            get
            {
                return getId();
            }
        }

        /// <summary>
        /// Gets or sets the ID of the recipe associated with this
        /// image
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
        /// Gets the path of the image
        /// </summary>
        public String ImagePath
        {
            get
            {
                return this.imagePath;
            }
            set
            {
                imagePath = value;
            }
        }

        /// <summary>
        /// Gets the internal bitmap image of this recipe image
        /// </summary>
        public BitmapImage InternalImage
        {
            get
            {
                return this.internalImage;
            }
        }

        public RecipeImage(BitmapImage internalImage)
        {
            this.id = RecipeList.imageIdGenerator.getId();
            this.internalImage = internalImage;
            recipeId = -1;
        }

        public RecipeImage(long id, BitmapImage internalImage)
        {
            this.id = id;
            this.internalImage = internalImage;
            recipeId = -1;
        }

        public RecipeImage(long id, String path)
        {
            createImageFromPath(path);
            this.imagePath = path;
            this.id = id;
        }

        private async void createImageFromPath(String path)
        {
            StorageFile imageFile = await StorageFile.GetFileFromPathAsync(path);
            if (imageFile != null)
            {
                BitmapImage image = null;
                if (imageFile.IsAvailable)
                {
                    using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
                    {
                        image = new BitmapImage();
                        await image.SetSourceAsync(stream);
                        this.internalImage = image;
                        stream.Dispose();
                    }
                }
                else
                {
                    Uri imageUri = new Uri(imageFile.Path, UriKind.Absolute);
                    image = new BitmapImage();
                    image.UriSource = imageUri;
                    this.internalImage = image;
                }
            }
        }

        /// <summary>
        /// Gets the ID of this recipe image
        /// </summary>
        /// <returns>
        /// the id of this image
        /// </returns>
        public long getId()
        {
            return id;
        }

        /// <summary>
        /// Gets the ID of the recipe this image is attached to
        /// </summary>
        /// <returns>
        /// the id of the associated recipe
        /// </returns>
        public long getRecipeId()
        {
            return recipeId;
        }

        public Uri getImageUri()
        {
            return new Uri(this.imagePath);
        }

        /// <summary>
        /// Sets the recipe id of this image to the given id
        /// </summary>
        /// <param name="recipeId">
        /// the id of the recipe to associate
        /// </param>
        public void setRecipeId(long recipeId)
        {
            this.recipeId = recipeId;
        }

        public void setInternalImage(BitmapImage newImage)
        {
            this.internalImage = newImage;
        }
    }
}