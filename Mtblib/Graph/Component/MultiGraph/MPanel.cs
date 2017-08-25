using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.MultiGraph
{
    public class MPanel : Label
    {
        public MPanel()
        {
            SetDefault();
        }

        private string[] _paneledBy = null;
        /// <summary>
        /// 指定或取得 Panel 的分群變數，可輸入時可一(string)或多個(stirng[])欄位文字，以string[] 回傳
        /// </summary>
        public dynamic PaneledBy
        {
            set
            {
                _paneledBy = MtbTools.ConvertInputToStringArray(value);
            }
            get
            {
                return _paneledBy;
            }
        }

        private int[] _rc = null;
        /// <summary>
        /// 指定或取得 Panel 個分割列數和行數，依序為 row number(int) 和 column number(int)
        /// </summary>
        public int[] RowColumn
        {
            set
            {
                if (value != null && value.Length != 2) throw new ArgumentException("RC 有不正確的參數個數，必須為 2 個!");
                _rc = MtbTools.ConvertInputToIntArray(value);
            }
            get
            {
                return _rc;
            }
        }

        /// <summary>
        /// 指定或取得 Panel 的座標軸顯示是否要交叉顯示
        /// </summary>
        public bool Alternative { set; get; }

        public override void SetDefault()
        {
            FontColor = -1;
            FontSize = -1;
            Bold = false;
            Italic = false;
            Underline = false;
            Alternative = false;
            PaneledBy = null;
            RowColumn = null;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (this.PaneledBy == null) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendFormat(" Panel {0};\r\n", string.Join(" &\r\n", this.PaneledBy));
            if (RowColumn != null) cmnd.AppendFormat(" RC {0} {1};\r\n", RowColumn[0], RowColumn[1]);
            cmnd.AppendLine(" Label;");
            if (FontSize > 0) cmnd.AppendFormat(" Psize {0};\r\n", FontSize);
            if (FontColor > 0) cmnd.AppendFormat(" TColor {0};\r\n", FontColor);
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Underline;");
            if (!Alternative) cmnd.AppendLine(" Noalter;");
            return cmnd.ToString();

        }

        public object Clone()
        {
            MPanel obj = new MPanel();
            if (this.PaneledBy != null) obj.PaneledBy = (string[])this.PaneledBy.Clone();
            if (this.RowColumn != null) obj.RowColumn = (int[])this.RowColumn.Clone();
            obj.FontSize = this.FontSize;
            obj.FontColor = this.FontColor;
            obj.Bold = this.Bold;
            obj.Italic = this.Italic;
            obj.Underline = this.Underline;
            obj.Alternative = this.Alternative;
            //obj.GetCommand = this.GetCommand;
            return obj;

        }

        [Obsolete("Panel 不支援 Align", true)]
        public new Align Alignment
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

        [Obsolete("Panel 不支援 Offset", true)]
        public new double[] Offset
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

        [Obsolete("Panel 不支援 Placement", true)]
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

        [Obsolete("Panel 不支援 Angle", true)]
        public new double Angle
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
