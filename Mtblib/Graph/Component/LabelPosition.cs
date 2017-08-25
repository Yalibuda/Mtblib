using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component
{
    public class LabelPosition : Label, IPosition
    {
        public LabelPosition()
        {
            SetDefault();
        }

        public LabelPosition(int rowid, string lab = null)
        {
            SetDefault();
            RowId = rowid;
            Text = lab;
        }

        public override void SetDefault()
        {
            RowId = null;
            Model = -1;
            Text = null;
            FontColor = -1;
            FontSize = -1;
            Angle = MtbTools.MISSINGVALUE;
            Bold = false;
            Italic = false;
            Underline = false;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (RowId == null) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendFormat(" Posi {0} {1};\r\n", RowId[0], (Text == null ? "" : "\"" + Text + "\""));
            if (FontColor > 0) cmnd.AppendFormat("  TColor {0};\r\n", FontColor);
            if (FontSize > 0) cmnd.AppendFormat("  PSize {0};\r\n", FontSize);
            if (Bold) cmnd.AppendLine("  Bold;");
            if (Italic) cmnd.AppendLine("  Italic;");
            if (Underline) cmnd.AppendLine("  Underline;");
            if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendFormat("  Angle {0};\r\n", Angle);
            if (Offset != null) cmnd.AppendFormat("  Offset {0};\r\n", string.Join(" &\r\n", Offset));
            if (Placement != null) cmnd.AppendFormat("  Placement {0};\r\n", string.Join(" &\r\n", Placement));
            cmnd.AppendLine(" Endp;");

            return cmnd.ToString();
        }

        /// <summary>
        /// 設定 Position 指令中要調整多組資料繪圖時的 Panel 群組(e.g. y1*x1 y2*x1 繪製於同一個 Graph window)
        /// </summary>
        public int Model { set; get; }

        int[] _rowid = null;
        /// <summary>
        /// 設定 Minitab Position 指令中要微調的資料位置
        /// </summary>
        public dynamic RowId
        {
            get
            {
                return _rowid;
            }
            set
            {
                if (value != null & value is Array)
                {
                    if (value.Length > 1) throw new ArgumentException("TextPosition 只能輸入一個 RowId");
                }
                _rowid = MtbTools.ConvertInputToIntArray(value);
            }
        }

        public override object Clone()
        {
            LabelPosition obj = new LabelPosition();
            obj.Alignment = this.Alignment;
            obj.Angle = this.Angle;
            obj.Bold = this.Bold;
            obj.Italic = this.Italic;
            obj.Underline = this.Underline;
            obj.Visible = this.Visible;           
            obj.FontSize = this.FontSize;
            obj.FontColor = this.FontColor;
            obj.Model = this.Model;
            if (this.RowId != null) obj.RowId = this.RowId.Clone();
            if (this.Offset != null) obj.Offset = (double[])this.Offset.Clone();
            if (this.Placement != null) obj.Placement = (double[])this.Placement.Clone();
            
            return obj;
        }

    }
}
