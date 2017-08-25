using Mtblib.Graph.Component;
using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.CategoricalChart
{
    public class Meanlab : Datlab
    {
        protected override string DefaultCommand()
        {
            if (!Visible) return "";
            if (DatlabType == DisplayType.Column && LabelColumn == null) return "# 未指定給 Datlab 欄位\r\n";
            StringBuilder cmnd = new StringBuilder();
            switch (DatlabType)
            {
                case DisplayType.YValue:
                    cmnd.AppendLine(" Mealab;");
                    cmnd.AppendLine("  YValue;");
                    break;
                case DisplayType.RowNumber:
                    cmnd.AppendLine(" Mealab;");
                    cmnd.AppendLine("  Rownum;");
                    break;
                case DisplayType.Column:
                    cmnd.AppendFormat(" Mealab {0};\r\n", LabelColumn);
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
    }
}
