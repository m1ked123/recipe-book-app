using Recipe_Book.Models;
using Recipe_Book.Utils;
using Recipe_Book.ViewModels;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Recipe_Book
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application

    {
        public static RecipeList recipes;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            recipes = new RecipeList();

            recipes.verifyImageFolder();

            RecipeBookDataAccessor.InitializeDatabase();

            long recipeStartingId = RecipeBookDataAccessor.getMaxId(Recipe.TABLE_NAME) + 1;
            RecipeList.recipeIdGenerator = new IdentifierGenerator(Recipe.TABLE_NAME, recipeStartingId);

            long ingredientStartingId = RecipeBookDataAccessor.getMaxId(RecipeIngredient.TABLE_NAME) + 1;
            RecipeList.ingredientIdGenerator = new IdentifierGenerator(RecipeIngredient.TABLE_NAME, ingredientStartingId);

            long stepStartingId = RecipeBookDataAccessor.getMaxId(RecipeStep.TABLE_NAME) + 1;
            RecipeList.stepIdGenerator = new IdentifierGenerator(RecipeStep.TABLE_NAME, stepStartingId);

            long imageStartingId = RecipeBookDataAccessor.getMaxId(RecipeImage.TABLE_NAME) + 1;
            RecipeList.imageIdGenerator = new IdentifierGenerator(RecipeImage.TABLE_NAME, imageStartingId);

            long entryStartingId = RecipeBookDataAccessor.getMaxId(RecipeJournalEntry.TABLE_NAME) + 1;
            RecipeList.journalEntryIdGenerator = new IdentifierGenerator(RecipeJournalEntry.TABLE_NAME, entryStartingId);

            ObservableCollection<Recipe> savedRecipes = RecipeBookDataAccessor.getSavedRecipes();
            for (int i = 0; i < savedRecipes.Count; i++)
            {
                Recipe savedRecipe = savedRecipes[i];
                ObservableCollection<RecipeIngredient> savedIngredients = RecipeBookDataAccessor.getIngredients(savedRecipe.ID);
                ObservableCollection<RecipeStep> savedSteps = RecipeBookDataAccessor.getSteps(savedRecipe.ID);
                ObservableCollection<RecipeImage> savedImages = RecipeBookDataAccessor.getImages(savedRecipe.ID);
                RecipeJournal savedEntries = RecipeBookDataAccessor.getJournalEntries(savedRecipe.ID);
                savedRecipe.setIngredients(savedIngredients);
                savedRecipe.setSteps(savedSteps);
                savedRecipe.setImages(savedImages);
                savedRecipe.setJournalEntries(savedEntries);
            }
            recipes.setRecipeList(savedRecipes);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage));
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            //var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 72, 169, 153);
            titleBar.ButtonPressedForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 0, 76, 64);

            // Set active window colors
            /*
            titleBar.ForegroundColor = Colors.White;
            titleBar.BackgroundColor = Color.FromArgb(255, 0, 121, 107);
            
            */
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
