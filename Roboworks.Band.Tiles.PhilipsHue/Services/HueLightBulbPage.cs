using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Roboworks.Band.Tiles.PhilipsHue.Services
{
	internal class HueLightBulbPage
	{
		private readonly PageLayout pageLayout;
		private readonly PageLayoutData pageLayoutData;
		
		private readonly ScrollFlowPanel panel = new ScrollFlowPanel();
		private readonly FlowPanel panel2 = new FlowPanel();
		internal TextBlock BulbName = new TextBlock();
		internal TextBlock Brightness = new TextBlock();
		private readonly FlowPanel panel3 = new FlowPanel();
		internal TextButton DimButton = new TextButton();
		internal TextButton OnOffButton = new TextButton();
		internal TextButton BrightenButton = new TextButton();
		
		internal TextBlockData BulbNameData = new TextBlockData(3, "{0}");
		internal TextBlockData BrightnessData = new TextBlockData(4, "Brightness: {0}%");
		internal TextButtonData DimButtonData = new TextButtonData(6, "<");
		internal TextButtonData OnOffButtonData = new TextButtonData(7, "ON / OFF");
		internal TextButtonData BrightenButtonData = new TextButtonData(8, ">");
		
		public HueLightBulbPage()
		{
			LoadIconMethod = LoadIcon;
			AdjustUriMethod = (uri) => uri;
			
			panel = new ScrollFlowPanel();
			panel.ScrollBarColorSource = ElementColorSource.Custom;
			panel.ScrollBarColor = new BandColor(255, 255, 255);
			panel.Orientation = FlowPanelOrientation.Vertical;
			panel.Rect = new PageRect(0, 0, 258, 128);
			panel.ElementId = 1;
			panel.Margins = new Margins(0, 0, 0, 0);
			panel.HorizontalAlignment = HorizontalAlignment.Left;
			panel.VerticalAlignment = VerticalAlignment.Top;
			
			panel2 = new FlowPanel();
			panel2.Orientation = FlowPanelOrientation.Vertical;
			panel2.Rect = new PageRect(0, 0, 243, 122);
			panel2.ElementId = 2;
			panel2.Margins = new Margins(6, 6, 0, 0);
			panel2.HorizontalAlignment = HorizontalAlignment.Left;
			panel2.VerticalAlignment = VerticalAlignment.Top;
			
			BulbName = new TextBlock();
			BulbName.Font = TextBlockFont.Small;
			BulbName.Baseline = 30;
			BulbName.BaselineAlignment = TextBlockBaselineAlignment.Absolute;
			BulbName.AutoWidth = true;
			BulbName.ColorSource = ElementColorSource.BandHighlight;
			BulbName.Rect = new PageRect(0, 0, 32, 30);
			BulbName.ElementId = 3;
			BulbName.Margins = new Margins(0, 0, 0, 0);
			BulbName.HorizontalAlignment = HorizontalAlignment.Left;
			BulbName.VerticalAlignment = VerticalAlignment.Top;
			
			panel2.Elements.Add(BulbName);
			
			Brightness = new TextBlock();
			Brightness.Font = TextBlockFont.Small;
			Brightness.Baseline = 60;
			Brightness.BaselineAlignment = TextBlockBaselineAlignment.Absolute;
			Brightness.AutoWidth = true;
			Brightness.ColorSource = ElementColorSource.Custom;
			Brightness.Color = new BandColor(255, 255, 255);
			Brightness.Rect = new PageRect(0, 0, 32, 30);
			Brightness.ElementId = 4;
			Brightness.Margins = new Margins(0, 0, 0, 0);
			Brightness.HorizontalAlignment = HorizontalAlignment.Left;
			Brightness.VerticalAlignment = VerticalAlignment.Top;
			
			panel2.Elements.Add(Brightness);
			
			panel3 = new FlowPanel();
			panel3.Orientation = FlowPanelOrientation.Horizontal;
			panel3.Rect = new PageRect(0, 0, 243, 48);
			panel3.ElementId = 5;
			panel3.Margins = new Margins(0, 8, 0, 0);
			panel3.HorizontalAlignment = HorizontalAlignment.Left;
			panel3.VerticalAlignment = VerticalAlignment.Top;
			
			DimButton = new TextButton();
			DimButton.PressedColor = new BandColor(32, 32, 32);
			DimButton.Rect = new PageRect(0, 0, 48, 48);
			DimButton.ElementId = 6;
			DimButton.Margins = new Margins(0, 0, 0, 0);
			DimButton.HorizontalAlignment = HorizontalAlignment.Center;
			DimButton.VerticalAlignment = VerticalAlignment.Top;
			
			panel3.Elements.Add(DimButton);
			
			OnOffButton = new TextButton();
			OnOffButton.PressedColor = new BandColor(32, 32, 32);
			OnOffButton.Rect = new PageRect(0, 0, 129, 48);
			OnOffButton.ElementId = 7;
			OnOffButton.Margins = new Margins(8, 0, 8, 0);
			OnOffButton.HorizontalAlignment = HorizontalAlignment.Center;
			OnOffButton.VerticalAlignment = VerticalAlignment.Top;
			
			panel3.Elements.Add(OnOffButton);
			
			BrightenButton = new TextButton();
			BrightenButton.PressedColor = new BandColor(32, 32, 32);
			BrightenButton.Rect = new PageRect(0, 0, 48, 48);
			BrightenButton.ElementId = 8;
			BrightenButton.Margins = new Margins(0, 0, 0, 0);
			BrightenButton.HorizontalAlignment = HorizontalAlignment.Center;
			BrightenButton.VerticalAlignment = VerticalAlignment.Top;
			
			panel3.Elements.Add(BrightenButton);
			
			panel2.Elements.Add(panel3);
			
			panel.Elements.Add(panel2);
			pageLayout = new PageLayout(panel);
			
			PageElementData[] pageElementDataArray = new PageElementData[5];
			pageElementDataArray[0] = BulbNameData;
			pageElementDataArray[1] = BrightnessData;
			pageElementDataArray[2] = DimButtonData;
			pageElementDataArray[3] = OnOffButtonData;
			pageElementDataArray[4] = BrightenButtonData;
			
			pageLayoutData = new PageLayoutData(pageElementDataArray);
		}
		
		public PageLayout Layout
		{
			get
			{
				return pageLayout;
			}
		}
		
		public PageLayoutData Data
		{
			get
			{
				return pageLayoutData;
			}
		}
		
		public Func<string, Task<BandIcon>> LoadIconMethod
		{
			get;
			set;
		}
		
		public Func<string, string> AdjustUriMethod
		{
			get;
			set;
		}
		
		private static async Task<BandIcon> LoadIcon(string uri)
		{
			StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));
			
			using (IRandomAccessStream fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
			{
				WriteableBitmap bitmap = new WriteableBitmap(1, 1);
				await bitmap.SetSourceAsync(fileStream);
				return bitmap.ToBandIcon();
			}
		}
		
		public async Task LoadIconsAsync(BandTile tile)
		{
			await Task.Run(() => { }); // Dealing with CS1998
		}
		
		public static BandTheme GetBandTheme()
		{
			var theme = new BandTheme();
			theme.Base = new BandColor(51, 102, 204);
			theme.HighContrast = new BandColor(58, 120, 221);
			theme.Highlight = new BandColor(58, 120, 221);
			theme.Lowlight = new BandColor(49, 101, 186);
			theme.Muted = new BandColor(43, 90, 165);
			theme.SecondaryText = new BandColor(137, 151, 171);
			return theme;
		}
		
		public static BandTheme GetTileTheme()
		{
			var theme = new BandTheme();
			theme.Base = new BandColor(51, 102, 204);
			theme.HighContrast = new BandColor(58, 120, 221);
			theme.Highlight = new BandColor(58, 120, 221);
			theme.Lowlight = new BandColor(49, 101, 186);
			theme.Muted = new BandColor(43, 90, 165);
			theme.SecondaryText = new BandColor(137, 151, 171);
			return theme;
		}
		
		public class PageLayoutData
		{
			private readonly PageElementData[] array;
			
			public PageLayoutData(PageElementData[] pageElementDataArray)
			{
				array = pageElementDataArray;
			}
			
			public int Count
			{
				get
				{
					return array.Length;
				}
			}
			
			public T Get<T>(int i) where T : PageElementData
			{
				return (T)array[i];
			}
			
			public T ById<T>(short id) where T:PageElementData
			{
				return (T)array.FirstOrDefault(elm => elm.ElementId == id);
			}
			
			public PageElementData[] All
			{
				get
				{
					return array;
				}
			}
		}
		
	}
}
