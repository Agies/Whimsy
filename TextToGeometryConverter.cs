using System;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace WhimsyEarlierLiteracy
{
    public class TextToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;
            var fontsize = int.Parse((string)parameter);
            CultureInfo info = CultureInfo.CurrentUICulture;
            var flowDirection = FlowDirection.LeftToRight;
            var fontFamily = new FontFamily((string)Application.Current.Resources["DefaultFontFamily"]);
            var path = new GeometryGroup();
            double scale = fontsize/(double)32;
            var point = new Point();
            
            return path;
        }

        private void RenderFragment(object font, double scale, ref object offsetX, object offsetY, ref object curPoint, string renderText)
        {
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}