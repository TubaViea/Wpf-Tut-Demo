using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp;
public class DataPoint {
  public DateTimeOffset Timestamp { get; set; }
  public double Value { get; set; }

  ~DataPoint() {
    GC.SuppressFinalize(this);
  }
}
