using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.Scale
{
    public interface IScale
    {
        double Min { set; get; }
        double Max { set; get; }
        Tick Ticks { set; get; }
        Refe Refes { set; get; }
        AxLabel Label { set; get; }
        int[] LDisplay { set; get; }
        int[] HDisplay { set; get; }
        ScaleDirection Direction { set; get; }
        object Clone();
        
    }
}
