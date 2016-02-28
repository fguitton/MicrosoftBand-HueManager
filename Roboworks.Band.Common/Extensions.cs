using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

using Microsoft.Practices.Unity;

namespace Roboworks.Band.Common
{
    public static class Extensions
    {

#region

        /// <summary>
        /// Call this method to supress CS4014 warning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncOperation"></param>
        public static void Forget<T>(this IAsyncOperation<T> asyncOperation)
        {
        }

#endregion

#region IUnityContainer

        public static void RegisterAsSingleton<T>(this IUnityContainer container, string name)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            container.RegisterAsSingleton(typeof(T), name);
        }

        public static void RegisterAsSingleton(this IUnityContainer container, Type type, string name)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            container.RegisterType(type, name, new ContainerControlledLifetimeManager());
        }

        public static void RegisterAsSingleton<TFrom, TTo>(this IUnityContainer container) 
            where TTo : TFrom
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterAsSingleton<TFrom, TTo>(this IUnityContainer container, string name) 
            where TTo : TFrom
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            container.RegisterType<TFrom, TTo>(name, new ContainerControlledLifetimeManager());
        }

#endregion

    }
}
