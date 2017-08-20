/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Bitcoin.Curses"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Bitcoin.Curses.Services;
using Bitcoin.Curses.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Bitcoin.Curses.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    internal class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            //services
            SimpleIoc.Default.Register<IRateSettingsApplyService, RateSettingsApplyService>();
            SimpleIoc.Default.Register<ICustomCurrencySymbolService, CustomCurrencySymbolService>();
            SimpleIoc.Default.Register<IDataProvideService, DataProvideService>();
            SimpleIoc.Default.Register<IBitcoinDataService, BitcoinDataService>();
            SimpleIoc.Default.Register<ICurrencyNavigateService, CurrencyNavigateService>();
            SimpleIoc.Default.Register<INetworkService, NetworkService>();

            //viewModels
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
            ServiceLocator.Current.GetInstance<MainViewModel>().Cleanup();
        }
    }
}