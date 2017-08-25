﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;
using Mtblib.Graph.Component;
using Mtblib.Graph.Component.DataView;

namespace Mtblib.Graph.CategoricalChart
{
    public class CMean: DataView
    {
        public CMean()
        {
            SetDefault();
        }
        protected override string DefaultCommand()
        {
            if (!this.Visible) return string.Empty;

            StringBuilder cmnd = new StringBuilder();

            if (_groupBy != null)
            {
                cmnd.AppendLine("CMean &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("CMean;");
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
            _type = null;
            _color = null;
            _size = null;
            _groupBy = null;
            Visible = true;
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            CMean cmean = new CMean();
            cmean.Type = _type.Clone();
            cmean.Color = _color.Clone();
            cmean.Size = _size.Clone();
            cmean.GroupingBy = _groupBy.Clone();
            cmean.Visible = this.Visible;
            return cmean;
        }
    }
}