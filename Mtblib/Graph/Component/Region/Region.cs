using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;
using Mtblib.Graph.Component;

namespace Mtblib.Graph.Component.Region
{
    public abstract class Region: DataView.DataView
    {
        protected double[] _coord;
        public abstract void SetCoordinate(params object[] args);
        public abstract double[] GetCoordinate();
        public bool AutoSize { get { return _auto; } set { _auto = value;} }
        protected bool _auto = true;

        [Obsolete("Region 不支援 View 屬性", true)]
        public new bool Visible { set; get; }
        [Obsolete("Region 不支援 Group 屬性", true)]
        public new dynamic GroupingBy { set; get; }
        [Obsolete("Region 不支援 Size 屬性", true)]
        public new dynamic Size { set; get; }
    }    
    
}
