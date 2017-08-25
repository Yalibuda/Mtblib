using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    public class AxLabel : Label
    {
        public AxLabel(ScaleDirection scaleDirection)
        {
            _scaleDirection = scaleDirection;
            SetDefault();
        }
        
        private string[] _multiLab = null;
        /// <summary>
        /// 在 Multi-level 下，設定或取得給個 level 的標籤，合法的輸入為單一(string)或多個文字(string[])。
        /// 使用 Get 取得 string[]。內容由第一個元素至最後一個分別代表最外層(level 1)至最內層標籤。
        /// </summary>
        public dynamic MultiLables
        {
            get
            {
                return _multiLab;
            }
            set
            {
                if (value is string)
                {
                    _multiLab = new string[] { value };
                }
                else
                {
                    try
                    {
                        _multiLab = ((string[])value).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _multiLab = null;
                    }
                }
            }
        }

        private ScaleDirection _scaleDirection;
        /// <summary>
        /// 設定或取得 Axlab 要編輯的是主座標軸或次座標軸
        /// </summary>
        public ScalePrimary ScalePrimary { set; get; }

        private int _side = 1;
        /// <summary>
        /// 設定或取得 Axlab 要顯示的位置，1= low side, 2=high side
        /// </summary>
        public int Side
        {
            set
            {
                if (value != 0 && value != 1 && value != 2)
                {
                    throw new ArgumentException(string.Format("{0} 不是合法的輸入值", value));
                }
                _side = value;
            }
            get
            {
                return _side;
            }
        }

        public override void SetDefault()
        {
            Text = null;
            ScalePrimary = ScalePrimary.Primary;
            FontSize = -1;
            FontColor = -1;
            Bold = false;
            Italic = false;
            Underline = false;
            Visible = true;
            Angle = MtbTools.MISSINGVALUE;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (!Visible)
            {
                cmnd.AppendLine(" Adisplay 0;");
                //cmnd.AppendLine(" LShow;");
            }
            else
            {
                cmnd.AppendFormat(" Adisplay {0};\r\n", Side);
            }
            if (MultiLables != null)
            {
                for (int i = 0; i < MultiLables.Length; i++)
                {
                    cmnd.AppendFormat(" Label \"{0}\";\r\n", MultiLables[i]);
                    cmnd.AppendFormat(" ALevel {0};\r\n",i+1);                    
                }
            }
            if (FontSize > 0) cmnd.AppendLine(string.Format(" Psize {0};", FontSize));
            if (FontColor > 0) cmnd.AppendLine(string.Format(" Tcolor {0};", FontColor));
            if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format(" Angle {0};", Angle));
            if (Offset != null) cmnd.AppendLine(string.Format("Offset {0};", string.Join(" ", Offset)));
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Under;");

            if (cmnd.Length > 0)
            {
                if (ScalePrimary == Component.ScalePrimary.Secondary) cmnd.Insert(0, " Secs;\r\n");
                cmnd.Insert(0, string.Format("AxLabel {0}{1};\r\n", (int)_scaleDirection, Text == null ? "" : " \"" + Text + "\""));
            }

            return cmnd.ToString();
        }

        public override object Clone()
        {
            AxLabel axlab = new AxLabel(_scaleDirection);
            axlab.Text = Text;
            axlab.FontSize = FontSize;
            axlab.FontColor = FontColor;
            axlab.Angle = Angle;
            axlab.Bold = Bold;
            axlab.Italic = Italic;
            axlab.Underline = Underline;
            axlab.Side = Side;
            axlab.Visible = this.Visible;
            if (Offset != null) axlab.Offset = (double[])Offset.Clone();
            axlab.ScalePrimary = ScalePrimary;

            return axlab;
        }

        [Obsolete("Axlabel 不支援 Align 屬性", true)]
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
        [Obsolete("Axlabel 不支援 Placement 屬性", true)]
        public new double[] Placement { set; get; }

    }
}
