using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component
{
    /// <summary>
    /// Minitab 圖形上的標題
    /// </summary>
    public class Title : Label
    {
        public Title()
        {
            SetDefault();
        }
        public override void SetDefault()
        {
            Text = null;
            FontSize = -1;
            FontColor = -1;
            Bold = false;
            Italic = false;
            Underline = false;
            Visible = true;
            Offset = null;
            Angle = MtbTools.MISSINGVALUE;
            Alignment = Align.Center;            
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (!Visible) return "Nodt;\r\n";
            StringBuilder cmnd = new StringBuilder();
            if (!string.IsNullOrEmpty(Text))
            {
                cmnd.AppendLine(string.Format("Title \"{0}\";", Text));
            }
            else
            {
                cmnd.AppendLine("Title;");
            }
            if (FontColor > 0) cmnd.AppendLine(string.Format(" TColor {0};", FontColor));
            if (FontSize > 0) cmnd.AppendLine(string.Format(" PSize {0};", FontSize));
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Underline;");

            cmnd.AppendLine(" " + Alignment.ToString() + ";");
            if (Offset != null) cmnd.AppendFormat(" Offset {0} {1};\r\n", Offset[0], Offset[1]);
            return cmnd.ToString();
        }

        /// <summary>
        /// 複製 Title 物件
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            Title obj = new Title();
            obj.Text = this.Text;
            obj.Visible = this.Visible;
            obj.Alignment = this.Alignment;
            obj.Angle = this.Angle;
            obj.Bold = this.Bold;
            obj.Italic = this.Italic;
            obj.Underline = this.Underline;
            obj.FontSize = this.FontSize;
            obj.FontColor = this.FontColor;
            if (this.Offset != null) obj.Offset = (double[])this.Offset.Clone();

            return obj;

        }

        [Obsolete("Title 不支援 Placement 屬性", true)]
        public new double[] Placement
        {
            set
            {
                throw new NotImplementedException();
            }
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
