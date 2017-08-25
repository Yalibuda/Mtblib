using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component
{
    /// <summary>
    /// 定義圖形中文字的屬性
    /// </summary>
    public interface ILabels
    {
        float FontSize { set; get; }
        int FontColor { set; get; }
        bool Bold { set; get; }
        bool Underline { set; get; }
        bool Italic { set; get; }
        double Angle { set; get; }
        double[] Offset { set; get; }
        double[] Placement { set; get; }
        object Clone();
    }
}
