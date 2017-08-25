using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component
{
    public interface IDataView
    {
        dynamic Type { set; get; }
        dynamic Size { set; get; }
        dynamic Color { set; get; }
        dynamic EType { set; get; }
        dynamic ESize { set; get; }
        dynamic EColor { set; get; }
        dynamic Base { set; get; }
        bool Visible { set; get; }
        dynamic GroupingBy { set; get; }
        object Clone();
    }
}
