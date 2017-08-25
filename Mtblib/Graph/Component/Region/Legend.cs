using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.Region
{
    public class Legend : Region, ILabels
    {
        public Legend()
        {
            SetDefault();
        }

        /// <summary>
        /// 設定 Legend 的位置 (4個座標值，依序為 xmin, xmax, ymin ymax)
        /// </summary>
        /// <param name="args"></param>
        public override void SetCoordinate(params object[] args)
        {            
            if (args != null && args.Length != 4) throw new ArgumentException("Legend 有不正確的參數個數，必須為 4 個!");            
            _coord = MtbTools.ConvertInputToDoubleArray(args);
        }

        /// <summary>
        /// 取得 Legend 位置
        /// </summary>
        /// <returns></returns>
        public override double[] GetCoordinate()
        {
            return _coord;
        }

        public bool HideLegend { set; get; }

        public List<LegendSection> Sections { set; get; }

        protected override string DefaultCommand()
        {
            if (HideLegend) return "Nolegend;\r\n";

            if (_coord == null && AutoSize == false) return ""; //表示要手動卻沒有輸入座標，直接跳出
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("Legend &");
            if (AutoSize)
            {
                cmnd.AppendLine(";");
            }
            else
            {
                cmnd.AppendLine(string.Join(" &\r\n", _coord) + ";");
            }

            if (Type != null)
            {
                cmnd.AppendLine(string.Format(" Type {0};", Type[0]));
            }
            if (Color != null)
            {
                cmnd.AppendLine(string.Format(" Color {0};", Color[0]));
            }
            if (EType != null)
            {
                cmnd.AppendLine(string.Format(" EType {0};", Type[0]));
            }
            if (EColor != null)
            {
                cmnd.AppendLine(string.Format(" EColor {0};", Color[0]));
            }
            if (ESize != null)
            {
                cmnd.AppendLine(string.Format(" ESize {0};", ESize[0]));
            }
            if (FontColor > 0) cmnd.AppendFormat(" TColor {0};\r\n", FontColor);
            if (FontSize > 0) cmnd.AppendFormat(" PSize {0};\r\n", FontSize);
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Underline;");
            if (HFontColor > 0) cmnd.AppendFormat(" HTColor {0};\r\n", FontColor);
            if (HFontSize > 0) cmnd.AppendFormat(" HPSize {0};\r\n", FontSize);
            if (HBold) cmnd.AppendLine(" HBold;");
            if (HItalic) cmnd.AppendLine(" HItalic;");
            if (HUnderline) cmnd.AppendLine(" HUnderline;");

            foreach (LegendSection sect in Sections)
            {
                cmnd.Append(sect.GetCommand());
            }

            return cmnd.ToString();
        }

        public override void SetDefault()
        {
            Type = null;
            Color = null;
            EType = null;
            EColor = null;
            ESize = null;
            FontColor = -1;
            FontSize = -1;
            Bold = false;
            Italic = false;
            Underline = false;
            HFontColor = -1;
            HFontSize = -1;
            HBold = false;
            HItalic = false;
            HUnderline = false;
            HideLegend = false;
            Sections = new List<LegendSection>();
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            Legend obj = new Legend();
            if (Type != null) obj.Type = this.Type.Clone();
            if (Color != null) obj.Color = this.Color.Clone();
            if (EType != null) obj.EType = this.EType.Clone();
            if (EColor != null) obj.EColor = this.EColor.Clone();
            if (ESize != null) obj.ESize = this.ESize.Clone();
            obj.FontSize = this.FontSize;
            obj.FontColor = this.FontColor;
            obj.Bold = this.Bold;
            obj.Italic = this.Italic;
            obj.Underline = this.Underline;
            obj.HFontSize = this.HFontSize;
            obj.HFontColor = this.HFontColor;
            obj.HBold = this.HBold;
            obj.HItalic = this.HItalic;
            obj.HUnderline = this.HUnderline;
            obj.HideLegend = this.HideLegend;
            double[] coord = this.GetCoordinate();
            if (coord != null) obj.SetCoordinate(coord[0], coord[1], coord[2], coord[3]);
            foreach (LegendSection sec in Sections)
            {
                obj.Sections.Add((LegendSection)sec.Clone());
            }
            return obj;
            
            
        }

        public float FontSize { get; set; }
        public int FontColor { get; set; }
        public bool Bold { get; set; }
        public bool Underline { get; set; }
        public bool Italic { get; set; }
        public float HFontSize { set; get; }
        public int HFontColor { set; get; }
        public bool HBold { set; get; }
        public bool HItalic { set; get; }
        public bool HUnderline { set; get; }


        [Obsolete("Legend 不支援 Angle", true)]
        public double Angle { get; set; }
        [Obsolete("Legend 不支援 Offset", true)]
        public double[] Offset { get; set; }
        [Obsolete("Legend 不支援 Placement", true)]
        public double[] Placement { get; set; }
    }
}
