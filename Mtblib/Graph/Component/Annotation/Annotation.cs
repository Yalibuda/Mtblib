using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Annotation
{
    public abstract class Annotation : IDataView, ILabels
    {

        protected int[] _type = null;
        protected int[] _color = null;
        protected float[] _size = null;
        protected string[] _groupBy = null;
        protected int[] _etype = null;
        protected int[] _ecolor = null;
        protected float[] _esize = null;
        protected string[] _coord = null;

        /// <summary>
        /// 設定 Annotation 的座標
        /// </summary>
        /// <param name="args"></param>
        public abstract void SetCoordinate(params object[] args);

        /// <summary>
        /// 取得 Annotation 的座標資訊
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetCoordinate();

        /// <summary>
        /// 回覆預設值
        /// </summary>
        public abstract void SetDefault();

        /// <summary>
        /// 取得預設的 Annotation 的指令碼
        /// </summary>
        /// <returns></returns>
        public abstract string DefaultCommand();

        /// <summary>
        /// 取得 Annotation 的指令碼
        /// </summary>
        public Func<string> GetCommand { set; get; }

        /// <summary>
        /// 指定或取得Annotation使用的單位，0: Figure unit, 1, 2, ...: 指定的Data unit
        /// </summary>
        public int Unit { set; get; }

        /// <summary>
        /// 取得或回傳 Type 值 (int[])
        /// </summary>
        public dynamic Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = MtbTools.ConvertInputToIntArray(value);
            }
        }

        /// <summary>
        /// 取得或回傳 Size 值 (float[])
        /// </summary>
        public dynamic Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = MtbTools.ConvertInputToFloatArray(value);
            }
        }

        /// <summary>
        /// 取得或回傳 Color 值 (int[])
        /// </summary>
        public dynamic Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = MtbTools.ConvertInputToIntArray(value);
            }
        }

        /// <summary>
        /// 取得或回傳 EType 值 (int[])
        /// </summary>
        public dynamic EType
        {
            get
            {
                return _etype;
            }
            set
            {
                _etype = MtbTools.ConvertInputToIntArray(value);
            }
        }

        /// <summary>
        /// 取得或回傳 ESize 值 (float[])
        /// </summary>
        public dynamic ESize
        {
            get
            {
                return _esize;
            }
            set
            {
                _esize = MtbTools.ConvertInputToFloatArray(value);
            }
        }

        /// <summary>
        /// 取得或回傳 EColor 值 (int[])
        /// </summary>
        public dynamic EColor
        {
            get
            {
                return _ecolor;
            }
            set
            {
                _ecolor = MtbTools.ConvertInputToIntArray(value);
            }
        }

        private double[] _bar = null;
        /// <summary>
        /// 設定或取得 Bar 投影到 Y 軸的位置，合法 Set 為單一(double)或多個(double[])設定值。
        /// Get 時丟出 double[]
        /// </summary>
        public dynamic Base
        {
            get
            {
                return _bar;
            }
            set
            {
                _bar = MtbTools.ConvertInputToDoubleArray(value);
            }
        }

        public bool Visible { set; get; }

        [Obsolete("Annotation 不支援 Grouping by", true)]
        public dynamic GroupingBy
        {
            set
            {
                throw new NotImplementedException("Annotation 不支援分群");
            }
            get
            {
                return null;
            }
        }

        public float FontSize { set; get; }
        public int FontColor { set; get; }
        public bool Bold { set; get; }
        public bool Underline { set; get; }
        public bool Italic { set; get; }
        public double Angle { set; get; }

        protected double[] _offset = null;
        /// <summary>
        /// Moves a title away from the position specified alignment
        /// 輸入一個整數陣列 int[2]，第一個是水平位移(-1~1)、第二個是垂直位移(-1~1)
        /// </summary>
        public double[] Offset
        {
            set
            {
                if (value != null && value.Length != 2) throw new ArgumentException("Offset 參數個數不正確，必須為2。");
                _offset = MtbTools.ConvertInputToDoubleArray(value);
            }
            get
            {
                return _offset;
            }
        }

        protected double[] _placement = null;
        /// <summary>
        /// Moves a title away from the position specified alignment
        /// 輸入一個整數陣列 int[2]，第一個是水平位移(-1~1)、第二個是垂直位移(-1~1)
        /// </summary>
        public double[] Placement
        {
            set
            {
                if (value != null && value.Length != 2) throw new ArgumentException("Placement 參數個數不正確，必須為2。");
                _placement = MtbTools.ConvertInputToDoubleArray(value);
            }
            get
            {
                return _placement;
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        
    }
    /// <summary>
    /// Minitab 圖形上標記用的線段
    /// </summary>
    public class Line : Annotation
    {
        public Line()
        {
            SetDefault();
        }
        public override void SetDefault()
        {
            _coord = null;
            _type = null;
            _color = null;
            _size = null;
            Unit = 1;
            GetCommand = DefaultCommand;
        }

        public override string DefaultCommand()
        {
            if (_coord == null) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("Line &");
            cmnd.AppendLine(string.Join(" &\r\n", _coord) + ";");
            if (Type != null)
            {
                cmnd.AppendLine(string.Format("Type {0};", Type[0]));
            }
            if (Color != null)
            {
                cmnd.AppendLine(string.Format("Color {0};", Color[0]));
            }
            if (Size != null)
            {
                cmnd.AppendLine(string.Format("Size {0};", Size[0]));
            }
            cmnd.AppendLine(string.Format("Unit {0};", Unit));
            return cmnd.ToString();
        }

        /// <summary>
        /// 指定 Annotation line 的座標值
        /// </summary>
        /// <param name="args">可為 C C (Minitab column id，x 座標資料, y 座標資料) 或是 K K K K (double，起點 x, 起點 y, 終點 x, 終點  y) 的形式</param>
        public override void SetCoordinate(params object[] args)
        {
            if (args.Length != 2 && args.Length != 4) throw new ArgumentException("Annotaion line 有不正確的參數個數");

            string[] coord = args.Where(x => x != null).Select(x => x.ToString()).
                Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

            if (coord.Length != args.Length)
            {
                throw new ArgumentException("Annotation line 座標不可包含 null 或空白");
            }
            _coord = coord;
        }

        public override string[] GetCoordinate()
        {
            return _coord;
        }
    }
    /// <summary>
    /// Minitab 圖形上標記用的點
    /// </summary>
    public class Marker : Annotation
    {
        public Marker()
        {
            SetDefault();
        }
        public override void SetDefault()
        {
            _coord = null;
            _type = null;
            _color = null;
            _size = null;
            Unit = 1;
            GetCommand = DefaultCommand;
        }

        public override string DefaultCommand()
        {
            if (_coord == null) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("Mark &");
            cmnd.AppendLine(string.Join(" &\r\n", _coord) + ";");
            if (Type != null)
            {
                cmnd.AppendLine(string.Format("Type {0};", Type[0]));
            }
            if (Color != null)
            {
                cmnd.AppendLine(string.Format("Color {0};", Color[0]));
            }
            if (Size != null)
            {
                cmnd.AppendLine(string.Format("Size {0};", Size[0]));
            }

            cmnd.AppendLine(string.Format("Unit {0};", Unit));
            return cmnd.ToString();
        }

        /// <summary>
        /// 指定 Annotation marker 的座標值
        /// </summary>
        /// <param name="args">可為 C C (Minitab column id，x 座標資料, y 座標資料) 或是 K K K K (double，起點 x, 起點 y, 終點 x, 終點  y) 的形式</param>
        public override void SetCoordinate(params object[] args)
        {
            if (args.Length != 2) throw new ArgumentException("Annotaion marker 有不正確的參數個數");

            string[] coord = args.Where(x => x != null).Select(x => x.ToString()).
                Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

            if (coord.Length != args.Length)
            {
                throw new ArgumentException("Annotation marker 座標不可包含 null 或空白");
            }
            _coord = coord;
        }

        public override string[] GetCoordinate()
        {
            return _coord;
        }
    }
    /// <summary>
    /// Minitab 圖形上標記用的矩形
    /// </summary>
    public class Rectangle : Annotation
    {
        public Rectangle()
        {
            SetDefault();
        }
        public override void SetDefault()
        {
            _coord = null;
            _type = null;
            _color = null;
            _size = null;
            Unit = 1;
            GetCommand = DefaultCommand;
        }

        public override string DefaultCommand()
        {
            if (_coord == null) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("Rect &");
            cmnd.AppendLine(string.Join(" &\r\n", _coord) + ";");
            if (Type != null)
            {
                cmnd.AppendLine(string.Format("Type {0};", Type[0]));
            }
            if (Color != null)
            {
                cmnd.AppendLine(string.Format("Color {0};", Color[0]));
            }
            if (EType != null)
            {
                cmnd.AppendLine(string.Format("EType {0};", Type[0]));
            }
            if (EColor != null)
            {
                cmnd.AppendLine(string.Format("EColor {0};", Color[0]));
            }
            if (ESize != null)
            {
                cmnd.AppendLine(string.Format("ESize {0};", Size[0]));
            }
            cmnd.AppendLine(string.Format("Unit {0};", Unit));
            return cmnd.ToString();
        }

        /// <summary>
        /// 指定 Annotation rectangle 的座標值
        /// </summary>
        /// <param name="args">可為 C C (Minitab column id，x 座標資料, y 座標資料) 或是 K K K K (double，起點 x, 起點 y, 終點 x, 終點  y) 的形式</param>
        public override void SetCoordinate(params object[] args)
        {
            if (args.Length != 4 && args.Length != 8) throw new ArgumentException("Annotaion rectangle 有不正確的參數個數，必須為 4 個或 8 個!");

            string[] coord;
            coord = args.Where(x => x != null).Select(x => x.ToString()).
                Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

            if (coord.Length != args.Length)
            {
                throw new ArgumentException("Annotation retangle 座標不可包含 null 或空白。");
            }
            double result;
            if (coord.Select(x => double.TryParse(x, out result)).Any(x => x == false))
            {
                throw new ArgumentException("Annotation retangle 座標值必須為數字。");
            }
            _coord = coord;
        }

        public override string[] GetCoordinate()
        {
            return _coord;
        }
    }
    /// <summary>
    /// Minitab 圖形上標記用的文字方塊
    /// </summary>
    public class Textbox : Annotation
    {
        public Textbox()
        {
            SetDefault();
        }
        public string Text { set; get; }
        public override void SetDefault()
        {
            _coord = null;
            FontColor = -1;
            FontSize = -1;
            Italic = false;
            Bold = false;
            Underline = false;
            Angle = MtbTools.MISSINGVALUE;
            Unit = 1;
            Offset = null;
            Placement = null;
            this.Text = null;
            GetCommand = DefaultCommand;
        }

        public override string DefaultCommand()
        {
            if (_coord == null || string.IsNullOrEmpty(this.Text)) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("Text &");
            cmnd.AppendLine(string.Join(" &\r\n", _coord) + " &");
            cmnd.AppendLine(string.Format("{0};", this.Text));

            if (FontColor > 0) cmnd.AppendLine(string.Format("TColor {0};", FontColor));
            if (FontSize > 0) cmnd.AppendLine(string.Format("PSize {0};", FontSize));

            if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Angle {0};", Angle));
            if (Bold) cmnd.AppendLine("Bold;");
            if (Italic) cmnd.AppendLine("Italic;");
            if (Underline) cmnd.AppendLine("Under;");

            if (Offset != null)
            {
                cmnd.AppendLine(string.Format("Offset {0} {1};", Offset[0], Offset[1]));
            }

            if (Placement != null)
            {
                cmnd.AppendLine(string.Format("Placement {0} {1};", Placement[0], Placement[1]));
            }

            cmnd.AppendLine(string.Format("Unit {0};", Unit));
            return cmnd.ToString();
        }

        /// <summary>
        /// 指定 Annotation rectangle 的座標值
        /// </summary>
        /// <param name="args">可為 C C (Minitab column id，x 座標資料, y 座標資料) 或是 K K K K (double，起點 x, 起點 y, 終點 x, 終點  y) 的形式</param>
        public override void SetCoordinate(params object[] args)
        {
            if (args.Length != 2) throw new ArgumentException("Annotaion rectangle 有不正確的參數個數，必須為 2 個!");

            string[] coord;
            coord = args.Where(x => x != null).Select(x => x.ToString()).
                Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

            if (coord.Length != args.Length)
            {
                throw new ArgumentException("Annotation retangle 座標不可包含 null 或空白。");
            }
            _coord = coord;
        }

        public override string[] GetCoordinate()
        {
            return _coord;
        }
    }

}
