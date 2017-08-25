using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component
{
    public class Footnote : Label
    {
        public Footnote()
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
            Alignment = Align.Left;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (!Visible || string.IsNullOrEmpty(Text)) return "";

            StringBuilder cmnd = new StringBuilder();

            cmnd.AppendLine(string.Format("Footnote \"{0}\";", Text));
            if (FontColor > 0) cmnd.AppendLine(string.Format(" TColor {0};", FontColor));
            if (FontSize > 0) cmnd.AppendLine(string.Format(" PSize {0};", FontSize));
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Underline;");
            cmnd.AppendLine(" " + Alignment.ToString() + ";");
            return cmnd.ToString();
        }

        [Obsolete("Footnote 不支援 Placement 屬性", true)]
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
