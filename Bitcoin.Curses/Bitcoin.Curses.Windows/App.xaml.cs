using Bitcoin.Courses.Windows.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Bitcoin.Curses.Windows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            SettingsImplementation.Init();

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                //Color currentAccentColorHex = ((SolidColorBrush)Application.Current.Resources["SystemControlHighlightAccentBrush"]).Color;
                //Color currentInactiveAccentColorHex = ((Color)Application.Current.Resources["SystemDisabledWindowAppbarColor"]);

                //if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
                //{
                //    var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                //    if (titleBar != null)
                //    {
                //        titleBar.ButtonBackgroundColor = currentAccentColorHex;
                //        titleBar.ButtonForegroundColor = Colors.White;
                //        titleBar.BackgroundColor = currentAccentColorHex;
                //        titleBar.ForegroundColor = Colors.White;

                //        titleBar.InactiveBackgroundColor = currentInactiveAccentColorHex;
                //        titleBar.ButtonInactiveBackgroundColor = currentInactiveAccentColorHex;
                //    }
                //}

                ////Mobile customization
                //if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                //{
                //    var statusBar = StatusBar.GetForCurrentView();
                //    if (statusBar != null)
                //    {
                //        statusBar.BackgroundOpacity = 1;
                //        statusBar.BackgroundColor = currentAccentColorHex;
                //        statusBar.ForegroundColor = Colors.White;
                //    }
                //}

                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                Xamarin.Forms.Forms.Init(e);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            //if (ApiInformation.IsPropertyPresent(typeof(LaunchActivatedEventArgs).FullName, nameof(LaunchActivatedEventArgs.TileId)))
            //{
            //    // If clicked on from tile
            //    if (e.TileId != null)
            //    {
            //        //// If tile notification(s) were present
            //        //if (e.TileActivatedInfo.RecentlyShownNotifications.Count > 0)
            //        //{
            //        //    // Get arguments from the notifications that were recently displayed
            //        //    string currencyRedirectionArgument = e.TileActivatedInfo.RecentlyShownNotifications
            //        //    .Select(i => i.Arguments)
            //        //    .Where(x => x.Contains(Constants.CurrencyRedirectionArgumentSeparator) && x.StartsWith(Constants.CurrencyRedirectionArgumentCode))
            //        //    .FirstOrDefault();

            //        //    var redirectionToCurrency = string.IsNullOrEmpty(currencyRedirectionArgument) ? null : currencyRedirectionArgument.Split(Constants.CurrencyRedirectionArgumentSeparator[0]).Last();

            //        //    if (!string.IsNullOrEmpty(redirectionToCurrency))
            //        //    {
            //        //        Settings.SetLastViewedCurrency(redirectionToCurrency);
            //        //        ServiceLocator.Current.GetInstance<ICurrencyNavigateService>().NavigateToCurrency(redirectionToCurrency);
            //        //    }
            //        //}
            //    }
            //}

            // Ensure the current window is active
            Window.Current.Activate();
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

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}