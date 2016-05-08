using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Roboworks.Band.Common;
using Roboworks.Band.Tiles.PhilipsHue.Views;
using Roboworks.Band.Tiles.PhilipsHue.ViewModels;

namespace Roboworks.Band.Tiles.PhilipsHue
{
    public class PhilipsHueTileModel : ITileModel
    {
        internal const string ViewName = "PhilipsHueSetup";

        public string Title => ResourceManager.TileTitle;

        string ITileModel.ViewName => PhilipsHueTileModel.ViewName;

        public Type ViewType => typeof(PhilipsHueSetupView);

        public Type ViewModelType => typeof(PhilipsHueSetupViewModel);
    }
}
