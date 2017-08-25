using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component
{
    interface IPosition
    {
        int Model { set; get; }
        dynamic RowId { set; get; }
    }
}
