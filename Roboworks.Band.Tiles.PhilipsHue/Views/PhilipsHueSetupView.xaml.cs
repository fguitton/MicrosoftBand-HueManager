using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

using Roboworks.Band.Tiles.PhilipsHue.ViewModels;
using Roboworks.Band.Common;
using Roboworks.Band.Tiles.PhilipsHue.Controls;

namespace Roboworks.Band.Tiles.PhilipsHue.Views
{
    public sealed partial class PhilipsHueSetupView : Page
    {
        public PhilipsHueSetupView()
        {
            this.InitializeComponent();
        }
    }
}
