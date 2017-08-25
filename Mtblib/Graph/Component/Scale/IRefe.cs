using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.Scale
{
    public interface IRefe : ILabels, IDataView
    {
        dynamic Values { set; get; }
        dynamic Labels { set; get; }
        ScaleDirection Direction { get; }
        int Side { set; get; }
        bool Secondary { set; get; }
        object Clone();
    }
}
