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
    public class IQRBox : DataView
    {
        public IQRBox()
        {
            SetDefault();
        }
        protected override string DefaultCommand()
        {
            if (!this.Visible) return string.Empty;

            StringBuilder cmnd = new StringBuilder();

            if (_groupBy != null)
            {
                cmnd.AppendLine("IQRbox &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("IQRbox;");
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
            if (ESize != null)
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
            Visible = true;
            GetCommand = DefaultCommand;
        }
        public override object Clone()
        {
            IQRBox iqrBox = new IQRBox();
            iqrBox.Type = _type.Clone();
            iqrBox.Color = _color.Clone();
            iqrBox.EType = _etype.Clone();
            iqrBox.EColor = _ecolor.Clone();
            iqrBox.ESize = _esize.Clone();
            iqrBox.GroupingBy = _groupBy.Clone();
            iqrBox.Visible = this.Visible;
            return iqrBox;
        }
    }
}
