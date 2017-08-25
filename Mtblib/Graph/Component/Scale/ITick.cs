using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.Scale
{
    interface ITick : ILabels
    {
        int Start { set; get; }
        double Increament { set; get; }
        int Level { set; get; }
        int NMajor { set; get; }
        int NMinor { set; get; }
        void SetTicks(dynamic ticks);
        void SetLabels(dynamic labels);
        


    }
}
