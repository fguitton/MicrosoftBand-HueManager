﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Core;

namespace Roboworks.HueManager.Views
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            try
            {
                this.InitializeComponent();
            }
            catch(Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
