using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue
{
    public class HueException : Exception
    {
        public HueException()
        {
        }

        public HueException(string message) 
            : base(message)
        {
        }

        public HueException(string message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
