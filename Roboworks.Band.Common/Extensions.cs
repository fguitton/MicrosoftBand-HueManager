using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media;

using Microsoft.Practices.Unity;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Roboworks.Band.Common
{
    public static class Extensions
    {

#region Task

        /// <summary>
        /// Call this method to supress CS4014 warning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncOperation"></param>
        public static void Forget<T>(this IAsyncOperation<T> asyncOperation)
        {
        }

        /// <summary>
        /// Call this method to supress CS4014 warning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asyncOperation"></param>
        public static void Forget(this Task asyncOperation)
        {
        }

        #endregion

#region Math

        public static int ToPercentage(this double value)
        {
            if (value < 0 || value > 1)
            {
                throw 
                    new ArgumentOutOfRangeException(
                        "Passed value should be in range from 0 to 1 including."
                    );
            }

            int percentage;

            if (value > 0d && value < 0.01d)
            {
                percentage = 1;
            }
            else if (value > 0.99d && value < 1d)
            {
                percentage = 99;
            }
            else
            {
                percentage = (int)Math.Round(value * 100);
            }

            return percentage;
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

#region UIElement

        private static async Task<IRandomAccessStream> RenderToRandomAccessStreamAsync(UIElement element)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            
            await renderTargetBitmap.RenderAsync(element);    
 
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
 
            // Useful for rendering in the correct DPI
            var displayInformation = DisplayInformation.GetForCurrentView();
 
            var stream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetPixelData(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied,
                (uint)renderTargetBitmap.PixelWidth,
                (uint)renderTargetBitmap.PixelHeight,
                displayInformation.RawDpiX,
                displayInformation.RawDpiY,
                pixels
            );
 
            await encoder.FlushAsync();
            stream.Seek(0);
 
            return stream;
        }

        public static async Task<ImageSource> BlurEffectApply(this UIElement element, float blurAmount = 20f)
        {
            var imageSource = new BitmapImage();

            using (var stream = await Extensions.RenderToRandomAccessStreamAsync(element))
            {
                var device = new CanvasDevice();
                var bitmap = await CanvasBitmap.LoadAsync(device, stream);

                var renderer = 
                    new CanvasRenderTarget(
                        device,
                        bitmap.SizeInPixels.Width,
                        bitmap.SizeInPixels.Height, bitmap.Dpi
                    );

                using (var ds = renderer.CreateDrawingSession())
                {
                    var blur = new GaussianBlurEffect();
                    blur.BlurAmount = blurAmount;
                    blur.Source = bitmap;
                    ds.DrawImage(blur);
                }

                stream.Seek(0);
                await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

                imageSource.SetSource(stream);
            }

            return imageSource;
        }

#endregion

    }
}
