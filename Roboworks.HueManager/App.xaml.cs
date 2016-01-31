using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using Prism.Mvvm;
using Prism.Logging;

using Roboworks.HueManager.Views;
using Roboworks.HueManager.ViewModels;
using Roboworks.Hue;
using Roboworks.HueManager.Services;

namespace Roboworks.HueManager
{
    sealed partial class App : PrismUnityApplication
    {
        private readonly ImmutableDictionary<string, Type> _viewNameToTypeMappings =
            new Dictionary<string, Type>
            {
                [ViewNames.Main] = typeof(MainView),
                [ViewNames.HueSetup] = typeof(HueSetupView),
                [ViewNames.BandSetup] = typeof(BandSetupView)
            }
            .ToImmutableDictionary();

        private readonly ImmutableDictionary<Type, Type> _viewTypeToViewModelTypeMappings =
            new Dictionary<Type, Type>
            {
                [typeof(MainView)] = typeof(MainViewModel),
                [typeof(HueSetupView)] = typeof(HueSetupViewModel),
                [typeof(BandSetupView)] = typeof(BandSetupViewModel)
            }
            .ToImmutableDictionary();

        public App()
        {
            this.UnhandledException += this.Application_UnhandledException;

            this.InitializeComponent();
        }

        //TODO: Implement ILoggerFacade
        protected override ILoggerFacade CreateLogger()
        {
            return base.CreateLogger();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            this.NavigationService.Navigate(ViewNames.Main, null);
            return Task.CompletedTask;
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
                (type) => this._viewTypeToViewModelTypeMappings[type]
            );
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => this.Resolve(type));
        }

        protected override INavigationService OnCreateNavigationService(IFrameFacade rootFrame)
        {
            return new FrameNavigationService(rootFrame, this.ViewTypeGetter, this.SessionStateService);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.RegisterAsSingleton<ISettingsProvider, SettingsProvider>();

            // Services
            this.RegisterAsSingleton<IHueServiceProvider, HueServiceProvider>();
            this.RegisterAsSingleton<IBandService, BandService>();

            // Views
            this.RegisterAsSingleton<MainView>(ViewNames.Main);
            this.RegisterAsSingleton<HueSetupView>(ViewNames.HueSetup);
            this.RegisterAsSingleton<BandSetupView>(ViewNames.BandSetup);
        }

#region Private Methods

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionHandler.Handle(e.Exception);
        }

        private Type ViewTypeGetter(string viewName)
        {
            return this._viewNameToTypeMappings[viewName];
        }

        private void RegisterAsSingleton<T>(string name)
        {
            this.Container.RegisterType<T>(name, new ContainerControlledLifetimeManager());
        }

        private void RegisterAsSingleton<TFrom, TTo>() where TTo : TFrom
        {
            this.Container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }

#endregion

    }
}
