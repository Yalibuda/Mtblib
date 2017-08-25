using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;
using Mtblib.Graph.Component;
using Mtblib.Graph.Component.DataView;

namespace Mtblib.Graph.CategoricalChart
{
    public class RangeBox: DataView
    {
        public RangeBox()
        {
            SetDefault();
        }
        protected override string DefaultCommand()
        {
            if (!this.Visible) return string.Empty;

            StringBuilder cmnd = new StringBuilder();

            if (_groupBy != null)
            {
                cmnd.AppendLine("Rbox &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("Rbox;");
            }
            if (Type != null)
            {
                cmnd.AppendLine("Type &");
                cmnd.AppendLine(string.Join(" &\r\n", Type) + ";");
            }
            if (Color != null)
            {
                cmnd.AppendLine("Color &");
                cmnd.AppendLine(string.Join(" &\r\n", Color) + ";");
            }
            if (EType != null)
            {
                cmnd.AppendLine("EType &");
                cmnd.AppendLine(string.Join(" &\r\n", EType) + ";");
            }
            if (EColor != null)
            {
                cmnd.AppendLine("EColor &");
                cmnd.AppendLine(string.Join(" &\r\n", EColor) + ";");
            }
            if (Size != null)
            {
                cmnd.AppendLine("ESize &");
                cmnd.AppendLine(string.Join(" &\r\n", ESize) + ";");
            }
            return cmnd.ToString();
        }

        public override void SetDefault()
        {
            _type = null;
            _color = null;
            _etype = null;
            _ecolor = null;
            _esize = null;
            _groupBy = null;
            Visible = false;
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            RangeBox rngbox = new RangeBox();
            rngbox.Type = _type.Clone();
            rngbox.Color = _color.Clone();
            rngbox.EType = _etype.Clone();
            rngbox.EColor = _ecolor.Clone();
            rngbox.ESize = _esize.Clone();
            rngbox.GroupingBy = _groupBy.Clone();
            rngbox.Visible = this.Visible;
            return rngbox;
        }
    }
}
