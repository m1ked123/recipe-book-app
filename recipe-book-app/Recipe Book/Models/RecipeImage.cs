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
        /// <summary>
        /// The name of the table in which image data is placed
        /// </summary>
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

        /// <summary>
        /// Creates a new RecipeImage with the given bitmap image as
        /// the internal image. The ID will be negative and is used
        /// for any images that haven't been added to the app's db
        /// </summary>
        /// <param name="internalImage">
        /// the bitmap image for this recipe image
        /// </param>
        public RecipeImage(BitmapImage internalImage)
        {
            this.id = RecipeList.imageIdGenerator.getId();
            this.internalImage = internalImage;
            recipeId = -1;
        }

        /// <summary>
        /// Creates a new recipe image with the given non-negatice id
        /// and the given internal image.
        /// </summary>
        /// <param name="id">
        /// the id to use
        /// </param>
        /// <param name="internalImage">
        /// the internal image to use
        /// </param>
        public RecipeImage(long id, BitmapImage internalImage)
        {
            this.id = id;
            this.internalImage = internalImage;
            recipeId = -1;
        }

        /// <summary>
        /// An alternative constructor that creates a recipe image
        /// with the given id and from the provided absolute path.
        /// </summary>
        /// <param name="id">
        /// the id to use
        /// </param>
        /// <param name="path">
        /// the path for the image
        /// </param>
        public RecipeImage(long id, String path)
        {
            createImageFromPath(path);
            this.imagePath = path;
            this.id = id;
        }

        // Creates a new buffered image for this recipe image from the
        // given absolute path
        private async void createImageFromPath(String path)
        {
            StorageFile imageFile = await StorageFile.GetFileFromPathAsync(path);
            if (imageFile != null)
            {
                BitmapImage image = null;
                if (imageFile.IsAvailable)
                {
                    using (IRandomAccessStream stream = 
                        await imageFile.OpenAsync(FileAccessMode.Read))
                    {
                        image = new BitmapImage();
                        await image.SetSourceAsync(stream);
                        this.internalImage = image;
                        stream.Dispose();
                    }
                }
                else
                {
                    Uri imageUri = new Uri(imageFile.Path,
                        UriKind.Absolute);
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

        /// <summary>
        /// Gets a string version of the path for this RecipeImage
        /// </summary>
        /// <returns>
        /// a string version of the path for this image
        /// </returns>
        public String getImagePath()
        {
            return this.imagePath;
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

        /// <summary>
        /// Sets the internal image of this RecipeImage to the given
        /// buffered image
        /// </summary>
        /// <param name="newImage">
        /// the buffered image that will be used for this recipe image
        /// </param>
        public void setInternalImage(BitmapImage newImage)
        {
            if (newImage != null)
            {
                this.internalImage = newImage;
            }
        }
    }
}