using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataWpf.Model
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return "";
            DateTime initialValue = (DateTime)value;

            string convertedValue = "";
            convertedValue = initialValue.Date.Day.ToString() + ". " + initialValue.Date.Month.ToString() + ". " + initialValue.Date.Year.ToString() + ".";

            return convertedValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime convertedValue = DateTime.MinValue;
            try
            {
                string uiValue = value as string;

                string[] parts = uiValue.Split('.');
                convertedValue = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
            }
            catch (Exception)
            {
                //ignoring errors
            }

            return convertedValue;

        }
    }
}
