using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp;
public static class Converters {
  [field: AllowNull, MaybeNull] public static IMultiValueConverter HorizontalOffset => field ??= new HoritontalOffsetConverter();
  [field: AllowNull, MaybeNull] public static IMultiValueConverter VerticalOffset => field ??= new VerticalOffsetConverter();
  [field: AllowNull, MaybeNull] public static IValueConverter Add => field ??= new AddConverter();
  [field: AllowNull, MaybeNull] public static IMultiValueConverter Divide => field ??= new DivideConverter();

  class HoritontalOffsetConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      var index = System.Convert.ToInt32(values[0]);
      var count = System.Convert.ToInt32(values[1]);
      var width = System.Convert.ToDouble(values[2]);
      var desc = $"{values[3]}";

      var r = width / count * index;
      //Debug.WriteLine($"h: {desc} {width} / {count} * {index} -> {r}");
      return r < 0 ? 0 : r;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  class VerticalOffsetConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      if (values[0] is not DataPoint pt)
        return 0.0;
      var height = System.Convert.ToDouble(values[1]);
      var desc = $"{values[2]}";

      var r = (1 - pt.Value) * height;
      //Debug.WriteLine($"v: {desc} {pt.Value} * {height} -> {r}");
      return r;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  class AddConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
      var x = System.Convert.ToDouble(value);
      var y = System.Convert.ToDouble(parameter);
      return x + y;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  class DivideConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      var x = System.Convert.ToDouble(values[0]);
      var y = System.Convert.ToDouble(values[1]);
      return x / y;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
