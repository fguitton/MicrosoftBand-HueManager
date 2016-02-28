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
using Roboworks.Band.Common;
using Roboworks.Band.Tiles.PhilipsHue;

namespace Roboworks.HueManager
{
    sealed partial class App : PrismUnityApplication
    {

        private readonly Dictionary<string, Type> _viewNameToTypeMappings =
            new Dictionary<string, Type>
            {
                [ViewNames.Main] = typeof(MainView)
            };

        private readonly Dictionary<Type, Type> _viewTypeToViewModelTypeMappings =
            new Dictionary<Type, Type>
            {
                [typeof(MainView)] = typeof(MainViewModel)
            };

        public App()
        {
            this.UnhandledException += this.Application_UnhandledException;

            this.InitializeComponent();
        }

#region Overiden Methods

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
            
            this.DependenciesRegister();
            this.ViewsRegister();
            this.ModulesRegister();
        }

#endregion

#region Private Methods

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionHandler.Handle(e.Exception);
            e.Handled = true;
        }

        private Type ViewTypeGetter(string viewName)
        {
            return this._viewNameToTypeMappings[viewName];
        }

        private void DependenciesRegister()
        {
            this.Container.RegisterAsSingleton<ISettingsProvider, SettingsProvider>();
        }

        private void ViewsRegister()
        {
            this.Container.RegisterAsSingleton<MainView>(ViewNames.Main);
        }

        private void ModulesRegister()
        {
            var modules = new IModule[] { new PhilipsHueModule() };
            foreach(var module in modules)
            {
                module.RegisterTypes(this.Container);
            }

            var tileModels = this.Container.ResolveAll<ITileModel>();
            foreach(var tileModel in tileModels)
            {
                this.Container.RegisterAsSingleton(tileModel.ViewType, tileModel.ViewName);

                this._viewNameToTypeMappings.Add(tileModel.ViewName, tileModel.ViewType);
                this._viewTypeToViewModelTypeMappings.Add(tileModel.ViewType, tileModel.ViewModelType);
            }
        }

#endregion

    }
}
