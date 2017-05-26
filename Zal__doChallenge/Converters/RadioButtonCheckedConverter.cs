using System;
using Windows.UI.Xaml.Data;

namespace Zal__doChallenge.Converters
{
	public class RadioButtonCheckedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value.Equals(parameter);
		}
	}
}
