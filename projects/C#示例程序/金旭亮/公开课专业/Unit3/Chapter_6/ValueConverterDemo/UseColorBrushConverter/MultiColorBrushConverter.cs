using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace UseColorBrushConverter
{
    [ValueConversion(typeof(int), typeof(SolidColorBrush))]
    class MultiColorBrushConverter : IMultiValueConverter
    {
        

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
            byte r = Byte.Parse(values[0].ToString());
            byte g=Byte.Parse(values[1].ToString());
            byte b = Byte.Parse(values[2].ToString());
            Color clr = Color.FromRgb(r,g, b);
            return new SolidColorBrush(clr);
            }
            catch 
            {

                return Binding.DoNothing;
            } 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
    }
}
