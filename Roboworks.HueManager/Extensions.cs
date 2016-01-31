using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

namespace Roboworks.HueManager
{
    public static class Extensions
    {

        /// <summary>
        /// Call this method to supress CS4014 warning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncOperation"></param>
        public static void Forget<T>(this IAsyncOperation<T> asyncOperation)
        {
        }

    }
}
