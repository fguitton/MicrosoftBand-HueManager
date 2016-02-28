using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Band.Common
{
    public interface ITileModel
    {

        string Title { get; }

        string ViewName { get; }

        Type ViewType { get; }

        Type ViewModelType { get; }
    }
}
