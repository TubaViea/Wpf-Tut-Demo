using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp;
/// <summary>
/// Interaction logic for GraphControl.xaml
/// </summary>
public partial class GraphControl : UserControl {
  #region DependencyProperty IList<DataPoint> DataPoints
  public static readonly DependencyProperty DataPointsProperty = DependencyProperty.Register(nameof(DataPoints), typeof(IList<DataPoint>), typeof(GraphControl),
    new FrameworkPropertyMetadata(default(IList<DataPoint>)));
  public IList<DataPoint> DataPoints { get => (IList<DataPoint>)GetValue(DataPointsProperty); set => SetValue(DataPointsProperty, value); }
  #endregion // DependencyProperty IList<DataPoint> DataPoints

  #region DependencyProperty Pen Pen
  public static readonly DependencyProperty PenProperty = DependencyProperty.Register(nameof(Pen), typeof(Pen), typeof(GraphControl),
    new FrameworkPropertyMetadata(default(Pen)));
  public Pen Pen { get => (Pen)GetValue(PenProperty); set => SetValue(PenProperty, value); }
  #endregion // DependencyProperty Pen Pen

  public GraphControl() {
    InitializeComponent();
  }
}
