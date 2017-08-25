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
    public class Outlier: DataView
    {
        public Outlier()
        {
            SetDefault();
        }
        protected override string DefaultCommand()
        {
            if (!this.Visible) return "";

            StringBuilder cmnd = new StringBuilder();

            if (_groupBy != null)
            {
                cmnd.AppendLine("Outlier &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("Outlier;");
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
            if (Size != null)
            {
                cmnd.AppendLine("Size &");
                cmnd.AppendLine(string.Join(" &\r\n", Size) + ";");
            }
            return cmnd.ToString();
        }

        public override void SetDefault()
        {
            Visible = true;
            _size = null;
            _color = null;
            _type = null;
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            Outlier outlier = new Outlier();
            outlier.Type = _type.Clone();
            outlier.Color = _color.Clone();
            outlier.Size = _size.Clone();            
            outlier.Visible = this.Visible;
            return outlier;
        }
    }
}
