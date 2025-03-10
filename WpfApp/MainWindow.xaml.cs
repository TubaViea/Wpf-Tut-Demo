﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp;

/// <summary>
///   Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
  #region DependencyProperty bool IsPaused
  public static readonly DependencyProperty IsPausedProperty = DependencyProperty.Register(nameof(IsPaused), typeof(bool), typeof(MainWindow),
    new FrameworkPropertyMetadata(default(bool)));
  public bool IsPaused { get => (bool)GetValue(IsPausedProperty); set => SetValue(IsPausedProperty, value); }
  #endregion // DependencyProperty bool IsPaused

  #region DependencyProperty int MaxPoints
  public static readonly DependencyProperty MaxPointsProperty = DependencyProperty.Register(nameof(MaxPoints), typeof(int), typeof(MainWindow),
    new FrameworkPropertyMetadata(42));
  public int MaxPoints { get => (int)GetValue(MaxPointsProperty); set => SetValue(MaxPointsProperty, value); }
  #endregion // DependencyProperty int MaxPoints

  #region DependencyProperty IList<DataPoint> DataPoints

  public static readonly DependencyProperty DataPointsProperty = DependencyProperty.Register(nameof(DataPoints), typeof(IList<DataPoint>), typeof(MainWindow),
    new FrameworkPropertyMetadata(default(IList<DataPoint>)));

  public IList<DataPoint> DataPoints { get => (IList<DataPoint>)GetValue(DataPointsProperty); set => SetValue(DataPointsProperty, value); }

  #endregion // DependencyProperty IList<DataPoint> DataPoints

  readonly CancellationTokenSource _Cts = new();
  Task _BackgroundTask = null!;

  public MainWindow() {
    DataPoints = new ObservableCollection<DataPoint>();
    InitializeComponent();
  }

  protected override void OnInitialized(EventArgs e) {
    base.OnInitialized(e);
    DataPointListBox.Loaded += loaded;
    _BackgroundTask = BackgroundTask(_Cts.Token);

    void loaded(object sender, RoutedEventArgs e) {
      var lb = (ItemsControl)sender!;
      var border = (Border)VisualTreeHelper.GetChild(lb, 0);
      var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
      ((INotifyCollectionChanged)lb.Items).CollectionChanged += dataPointsChanged;

      void dataPointsChanged(object? sender, NotifyCollectionChangedEventArgs e) => scrollViewer.ScrollToBottom();
    }
  }

  protected override void OnClosing(CancelEventArgs e) {
    _Cts.Cancel();
    base.OnClosing(e);
  }

  async Task BackgroundTask(CancellationToken ct) {
    try {
      var value = 0.0;
      for (; !ct.IsCancellationRequested;) {
        await Task.Delay(500, ct);
        if (IsPaused)
          continue;
        var now = DateTimeOffset.UtcNow;
        await Dispatcher.InvokeAsync(() => {
          while (DataPoints.Count > MaxPoints)
            DataPoints.RemoveAt(0);
          value += Random.Shared.NextDouble();
          value *= .5;
          DataPoints.Add(new() {
            Timestamp = now,
            Value = value,
          });
        }, DispatcherPriority.Background, ct);
      }
    } catch (TaskCanceledException) {
      // ignored.
    } catch (Exception exc) {
      Debug.WriteLine(exc);
    }
  }
}
