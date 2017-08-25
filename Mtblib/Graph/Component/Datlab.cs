using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component
{
    public class Datlab : Label
    {
        public Datlab()
        {
            SetDefault();
        }
        /// <summary>
        /// Data label 顯示內容，Yvalue、Rownumber 或特定的欄位
        /// </summary>
        public enum DisplayType
        {
            YValue = 1, RowNumber = 2, Column = 3
        }

        /// <summary>
        /// 微調的 Datlab 位置與屬性 List
        /// </summary>
        public List<LabelPosition> PositionList { set; get; }

        /// <summary>
        /// 設定 Datlab 要顯示哪種資訊，預設不顯示
        /// </summary>
        public DisplayType DatlabType { set; get; }

        /// <summary>
        /// 指定或取得 Datlab 要顯示的特定欄位
        /// </summary>
        public string LabelColumn { set; get; }

        public override void SetDefault()
        {
            Visible = false;
            FontSize = -1;
            FontColor = -1;
            Bold = false;
            Italic = false;
            Underline = false;
            Offset = null;
            Placement = null;
            Angle = MtbTools.MISSINGVALUE;
            PositionList = new List<LabelPosition>();
            DatlabType = DisplayType.YValue;
            LabelColumn = null;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (!Visible) return "";
            if (DatlabType == DisplayType.Column && LabelColumn == null) return "# 未指定給 Datlab 欄位\r\n";
            StringBuilder cmnd = new StringBuilder();
            switch (DatlabType)
            {
                case DisplayType.YValue:
                    cmnd.AppendLine(" Datlab;");
                    cmnd.AppendLine("  YValue;");
                    break;
                case DisplayType.RowNumber:
                    cmnd.AppendLine(" Datlab;");
                    cmnd.AppendLine("  Rownum;");
                    break;
                case DisplayType.Column:
                    cmnd.AppendFormat(" Datlab {0};\r\n", LabelColumn);
                    break;
            }
            if (FontColor > 0) cmnd.AppendFormat("  TColor {0};\r\n", FontColor);
            if (FontSize > 0) cmnd.AppendFormat("  PSize {0};\r\n", FontSize);
            if (Bold) cmnd.AppendLine("  Bold;");
            if (Italic) cmnd.AppendLine("  Italic;");
            if (Underline) cmnd.AppendLine("  Underline;");
            if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendFormat("  Angle {0};\r\n", Angle);
            if (Offset != null) cmnd.AppendFormat("  Offset {0};\r\n", string.Join(" &\r\n", Offset));
            if (Placement != null) cmnd.AppendFormat("  Placement {0};\r\n", string.Join(" &\r\n", Placement));

            foreach (LabelPosition pos in PositionList)
            {
                cmnd.Append(pos.GetCommand());
            }

            return cmnd.ToString();
        }

        public override object Clone()
        {
            Datlab obj = new Datlab();
            obj.Alignment = this.Alignment;
            obj.Angle = this.Angle;
            obj.Bold = this.Bold;
            obj.Italic = this.Italic;
            obj.Underline = this.Underline;
            obj.Visible = this.Visible;
            obj.LabelColumn = this.LabelColumn;
            obj.FontSize = this.FontSize;
            obj.FontColor = this.FontColor;
            obj.DatlabType = this.DatlabType;
            if (this.Offset != null) obj.Offset = (double[])this.Offset.Clone();
            if (this.Placement != null) obj.Placement = (double[])this.Placement.Clone();
            foreach (LabelPosition pos in PositionList)
            {
                if (pos != null) obj.PositionList.Add((LabelPosition)pos.Clone());
            }
            //obj.GetCommand = this.GetCommand;
            return obj;
        }
    }
}
