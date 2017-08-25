using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component
{
    public interface IDataOption
    {
        void GSave(string path);
        string WTitle { set; get; }

    }
}
