using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.DataView
{
    /// <summary>
    /// Dataview 的 Position 物件
    /// </summary>
    public class DataViewPosition : IDataView, IPosition
    {
        DataViewType _dvType = DataViewType.None;
        public enum DataViewType
        {
            None, Symbol, Conn, Project, Bar, Area
        }

        public DataViewPosition(IDataView dataview)
        {
            if (dataview is Symbol)
            {
                _dvType = DataViewType.Symbol;
            }
            else if (dataview is Connect)
            {
                _dvType = DataViewType.Conn;
            }
            else if (dataview is Projection)
            {
                _dvType = DataViewType.Project;
            }
            else if (dataview is Bar)
            {
                _dvType = DataViewType.Bar;
            }
            else
            {
                throw new ArgumentException("不合法的 IDataView 實作後類別，只可以是 Symbol, Conn, Project, Bar");
            }
            SetDefault();
        }
        public DataViewPosition(DataViewType dvType)
        {
            _dvType = dvType;
            SetDefault();
        }

        /// <summary>
        /// 設定或取得多組變數中要編輯的 Panel 
        /// </summary>
        public int Model { get; set; }

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
                if (value is string)
                {
                    // 判斷輸入內容是否包含連續表示式
                    System.Text.RegularExpressions.Regex regex =
                    new System.Text.RegularExpressions.Regex(@"(\d*)\s*:\s*(\d*)|(\d+)");
                    if (regex.IsMatch(value))
                    {
                        System.Text.RegularExpressions.MatchCollection matches =
                            regex.Matches(value);
                        List<int> lst = new List<int>();
                        foreach (System.Text.RegularExpressions.Match m in matches)
                        {
                            if (!string.IsNullOrEmpty(m.Groups[3].Value)) //單一數字
                            {
                                lst.Add(int.Parse(m.Groups[3].Value));
                            }
                            else //連續表示式
                            {
                                if (!string.IsNullOrEmpty(m.Groups[1].Value) &&
                                !string.IsNullOrEmpty(m.Groups[2].Value))
                                {
                                    int start = int.Parse(m.Groups[1].Value);
                                    int end = int.Parse(m.Groups[2].Value);
                                    int step = start > end ? -1 : 1;
                                    for (int i = start; (start <= end ? i <= end : i >= end); i += step)
                                    {
                                        lst.Add(i);
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("連續表示式的 : 前後必須要有數字");
                                }
                            }
                        }
                        _rowid = lst.ToArray();
                    }
                    else
                    {
                        throw new ArgumentException("輸入不合法的 Position row id");
                    }
                }
                else
                {
                    _rowid = MtbTools.ConvertInputToIntArray(value);
                }


            }
        }

        public void SetDefault()
        {
            Type = null;
            Color = null;
            Size = null;
            EType = null;
            EColor = null;
            ESize = null;
            Base = null;
            Model = -1;
            GetCommand = DefaultCommand;
        }
        /// <summary>
        /// 取得預設的 Minitab 指令碼
        /// </summary>
        /// <returns></returns>
        protected string DefaultCommand()
        {
            if (RowId == null) return "";
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendFormat("Posi {0};\r\n", string.Join(" &\r\n", RowId));
            if (Model > 0) cmnd.AppendFormat(" Model {0};\r\n", Model);
            if (Type != null) cmnd.AppendFormat(" Type {0};\r\n", Type);
            if (Color != null) cmnd.AppendFormat(" Color {0};\r\n", Color);
            switch (_dvType)
            {
                case DataViewType.Symbol:
                case DataViewType.Conn:
                case DataViewType.Project:
                    if (Size != null) cmnd.AppendFormat(" Size {0};\r\n", Size);
                    break;
            }
            switch (_dvType)
            {
                case DataViewType.Bar:
                    if (EType != null) cmnd.AppendFormat(" EType {0};\r\n", EType);
                    if (EColor != null) cmnd.AppendFormat(" EColor {0};\r\n", EColor);
                    if (ESize != null) cmnd.AppendFormat(" ESize {0};\r\n", ESize);
                    break;

            }
            switch (_dvType)
            {
                case DataViewType.Project:
                case DataViewType.Bar:
                    if (Base != null) cmnd.AppendFormat(" Base {0};\r\n", Base);
                    break;
            }

            cmnd.AppendLine("Endp;");
            return cmnd.ToString();
        }

        /// <summary>
        /// 取得 Minitab 指令碼
        /// </summary>
        public Func<string> GetCommand { set; get; }

        protected int? _type = null;
        /// <summary>
        /// 取得或回傳 Type 值
        /// </summary>
        public dynamic Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        protected float? _size = null;
        /// <summary>
        /// 取得或回傳 Size 值 
        /// </summary>
        public dynamic Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        protected int? _color = null;
        /// <summary>
        /// 取得或回傳 Color 值
        /// </summary>
        public dynamic Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        protected int? _etype = null;
        /// <summary>
        /// 取得或回傳 EType 值 
        /// </summary>
        public dynamic EType
        {
            get
            {
                return _etype;
            }
            set
            {
                _etype = value;
            }
        }

        protected float? _esize = null;
        /// <summary>
        /// 取得或回傳 ESize 值 
        /// </summary>
        public dynamic ESize
        {
            get
            {
                return _esize;
            }
            set
            {
                _esize = value;
            }
        }

        protected int? _ecolor = null;
        /// <summary>
        /// 取得或回傳 EColor 值 
        /// </summary>
        public dynamic EColor
        {
            get
            {
                return _ecolor;
            }
            set
            {
                _ecolor = value;
            }
        }

        protected double? _bar = null;
        /// <summary>
        /// 設定或取得 Bar 投影到 Y 軸的位置，合法 Set 為單一(double) 設定值。
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
                _bar = value;
            }
        }

        [Obsolete("Position 不支援 Visible 屬性", true)]
        public bool Visible
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
        [Obsolete("Position 不支援 GroupingBy 屬性", true)]
        public dynamic GroupingBy
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

        /// <summary>
        /// 深層複製
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //這裡的屬性都是 nullable 的 int/double/float 所以不用判斷是否為空
            DataViewPosition dvpos = new DataViewPosition(_dvType);
            dvpos.Base = this.Base;
            dvpos.Color = this.Color;
            dvpos.Type = this.Type;
            dvpos.Size = this.Size;
            dvpos.EType = this.EType;
            dvpos.EColor = this.EColor;
            dvpos.ESize = this.ESize;
            dvpos.Model = this.Model;
            if (this.RowId != null) dvpos.RowId = ((int[])this.RowId).Clone();
            return dvpos;
        }
    }
}
