using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Roboworks.Band.Common;
using Roboworks.Band.Tiles.PhilipsHue.Services;
using Roboworks.Hue;

namespace Roboworks.Band.Tiles.PhilipsHue
{
    public class PhilipsHueModule : IModule
    {
        public void RegisterTypes(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.RegisterAsSingleton<IBandService, BandService>();
            container.RegisterAsSingleton<IHueServiceProvider, HueServiceProvider>();

            container.RegisterAsSingleton<ITileModel, PhilipsHueTileModel>(PhilipsHueTileModel.ViewName);
        }
    }
}
