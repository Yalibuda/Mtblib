using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    public class Refe : Label, IRefe
    {


        public Refe(ScaleDirection direction)
            : base()
        {
            if (direction == ScaleDirection.None) throw new Exception("請指定 Reference 座標軸");
            _scaleDirection = direction;
            SetDefault();
        }

        public override void SetDefault()
        {
            Values = null;
            Secondary = false;
            Labels = null;
            Side = 2;
            FontColor = -1;
            FontSize = -1;
            Bold = false;
            Italic = false;
            Underline = false;
            Angle = MtbTools.MISSINGVALUE;
            Offset = null;
            Placement = null;
            Type = null;
            Color = null;
            Size = null;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (Values == null) return "";

            /*
             * 因為 Reference 輸入的模式有簡單和複雜，將由輸入的 Type, Color, Size 等屬性長度判斷，
             * command 的組成方式..
             * 1. 單一 value 或輸入欄位, 不管 Type, Color, Size ==> 簡單
             * 2. 多 value, 單一 Type, Color, Size ==> 簡單
             * 3. 多 value, 任一 Type, Color, Size 長度 >=2 ==> 複雜
             * 在處理複雜模式的時候，以 value 的長度為基數，以屬性長度 % 基數的結果取值
             * 
             * 注意: 複雜模式下，想讓每條線顯示不同的 Label，Label 請以string[] 方式輸入，勿以欄位名稱輸入
             */

            StringBuilder cmnd = new StringBuilder();
            if (Values.Length > 1 &&
                ((this.Type != null && this.Type.Length > 1) ||
                (this.Color != null && this.Color.Length > 1) ||
                (this.Size != null && this.Size.Length > 1)))
            {
                int vlength = Values.Length;
                for (int i = 0; i < Values.Length; i++)
                {
                    cmnd.AppendFormat("Refe {0} {1};\r\n", (int)Direction, Values[i]);
                    if (Secondary) cmnd.AppendLine(" Secs;");
                    cmnd.AppendLine(string.Format(" Side {0};", Side));
                    if (Labels != null) cmnd.AppendFormat(" Label \"{0}\";\r\n", this.Labels[i % vlength]);
                    if (Type != null) cmnd.AppendLine(string.Format(" Type {0};", this.Type[i % vlength]));
                    if (Color != null) cmnd.AppendLine(string.Format(" Color {0};", this.Color[i % vlength]));
                    if (Size != null) cmnd.AppendLine(string.Format(" Size {0};", this.Size[i % vlength]));

                    // 統一參數區
                    if (FontColor > 0) cmnd.AppendLine(string.Format(" TColor {0};", FontColor));
                    if (FontSize > 0) cmnd.AppendLine(string.Format(" PSize {0};", FontSize));
                    if (Bold) cmnd.AppendLine(" Bold;");
                    if (Italic) cmnd.AppendLine(" Italic;");
                    if (Underline) cmnd.AppendLine(" Underline;");
                    if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format(" Angle {0};", Angle));
                    if (Offset != null) cmnd.AppendLine(string.Format(" Offset {0} {1};", Offset[0], Offset[1]));
                    if (Placement != null) cmnd.AppendLine(string.Format(" Plac {0} {1};", Placement[0], Placement[1]));
                }
            }
            else
            {
                cmnd.AppendLine(string.Format("Refe {0} &", (int)_scaleDirection));
                cmnd.AppendLine(string.Join(" &\r\n", Values) + ";");
                if (Secondary) cmnd.AppendLine(" Secs;");
                cmnd.AppendLine(string.Format(" Side {0};", Side));
                if (Labels != null)
                {
                    cmnd.AppendLine(" Label &");
                    cmnd.AppendLine(string.Join(" &\r\n", ((string[])Labels).Select(x => "\"" + x + "\"")) + ";");
                }
                if (FontColor > 0) cmnd.AppendLine(string.Format(" TColor {0};", FontColor));
                if (FontSize > 0) cmnd.AppendLine(string.Format(" PSize {0};", FontSize));

                if (Bold) cmnd.AppendLine(" Bold;");
                if (Italic) cmnd.AppendLine(" Italic;");
                if (Underline) cmnd.AppendLine(" Underline;");
                if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format(" Angle {0};", Angle));
                if (Offset != null) cmnd.AppendLine(string.Format(" Offset {0} {1};", Offset[0], Offset[1]));
                if (Placement != null) cmnd.AppendLine(string.Format(" Plac {0} {1};", Placement[0], Placement[1]));

                if (Type != null) cmnd.AppendLine(string.Format(" Type {0};", Type[0]));
                if (Color != null) cmnd.AppendLine(string.Format(" Color {0};", Color[0]));
                if (Size != null) cmnd.AppendLine(string.Format(" Size {0};", Size[0]));
            }

            return cmnd.ToString();
        }

        private string[] _values = null;
        /// <summary>
        /// 設定或取得 Reference 的值，合法的指定為一個欄位名稱(string)，或一(double)或多個(double[])數值。
        /// 回傳類型為 string[]
        /// </summary>
        public dynamic Values
        {
            set
            {
                if (value is string)
                {
                    _values = new string[] { value };
                }
                else
                {
                    try
                    {
                        _values = ((double[])MtbTools.ConvertInputToDoubleArray(value)).
                        Select(x => x.ToString()).ToArray();
                    }
                    catch (Exception)
                    {
                        _values = null;
                    }
                }
            }
            get
            {
                return _values;
            }
        }

        private string[] _labels = null;
        /// <summary>
        /// 指定或取得 Reference 要顯示的標籤，合法的指定為單一欄位名稱(string)、單一(string)或多個(string[])包含雙引號的標籤值        
        /// </summary>
        public dynamic Labels
        {
            set
            {
                _labels = MtbTools.ConvertInputToStringArray(value);
            }
            get
            {
                return _labels;
            }
        }

        private ScaleDirection _scaleDirection = ScaleDirection.None;
        /// <summary>
        /// 指定或取得 Reference 的方向
        /// </summary>
        public ScaleDirection Direction
        {
            get { return _scaleDirection; }
        }
        /// <summary>
        /// 判斷該 Reference line 要使用主軸或副軸
        /// </summary>
        public bool Secondary { set; get; }
        /// <summary>
        /// 設定 Reference line 的 label 要顯示的位置，1: low side; 2: high side
        /// </summary>
        public int Side { set; get; }

        private string[] _type;
        /// <summary>
        /// 取得或回傳 Type 值 (string[])
        /// </summary>
        public dynamic Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value is string)
                {
                    _type = new string[] { value };
                }
                else
                {
                    try
                    {
                        _type = ((int[])MtbTools.ConvertInputToIntArray(value)).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _type = null;
                    }

                }
            }
        }


        private string[] _size;
        /// <summary>
        /// 取得或回傳 Size 值 (string[])
        /// </summary>
        public dynamic Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value is string)
                {
                    _size = new string[] { value };
                }
                else
                {
                    try
                    {
                        _size = ((float[])MtbTools.ConvertInputToFloatArray(value)).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _size = null;
                    }
                }

            }
        }

        private string[] _color;
        /// <summary>
        /// 取得或回傳 Color 值 (string[])
        /// </summary>
        public dynamic Color
        {
            get
            {
                return _color;
            }
            set
            {
                if (value is string)
                {
                    _color = new string[] { value };
                }
                else
                {
                    try
                    {
                        _color = ((int[])MtbTools.ConvertInputToIntArray(value)).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _color = null;
                    }

                }
            }
        }

        [Obsolete("Reference 不支援 Text 屬性，請使用 Labels 屬性", true)]
        public new string Text
        {
            set { throw new NotImplementedException(); }
            get { throw new NotImplementedException(); }
        }
        [Obsolete("Reference 不支援 EType", true)]
        public new dynamic EType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [Obsolete("Reference 不支援 ESize", true)]
        public new dynamic ESize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [Obsolete("Reference 不支援 EColor", true)]
        public new dynamic EColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [Obsolete("Reference 不支援 GroupingBy", true)]
        public new dynamic GroupingBy
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [Obsolete("Reference 不支援 Base", true)]
        public new dynamic Base
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override object Clone()
        {
            Refe obj = new Refe(this.Direction);
            obj.Secondary = this.Secondary;
            if (Values != null) obj.Values = this.Values.Clone();
            if (Labels != null) obj.Labels = this.Labels.Clone();
            if (Type != null) obj.Type = (string[])this.Type.Clone();
            if (Color != null) obj.Color = (string[])this.Color.Clone();
            if (Size != null) obj.Size = (string[])this.Size.Clone();
            obj.FontSize = this.FontSize;
            obj.FontColor = this.FontColor;
            obj.Bold = this.Bold;
            obj.Italic = this.Italic;
            obj.Angle = this.Angle;
            if (this.Offset != null) obj.Offset = (double[])Offset.Clone();
            if (this.Placement != null) obj.Offset = (double[])Placement.Clone();

            return obj;

            //(FontColor 
            //(FontSize >
            //(Bold) cmnd
            //(Italic) cm
            //(Underline)
            //(Angle < Mt
            //(Offset != 
            //(Placement 
        }

    }
}
