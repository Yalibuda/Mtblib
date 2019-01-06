using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.DataView
{
    public abstract class DataView : IDataView
    {
        protected string[] _type = null;
        protected string[] _color = null;
        protected string[] _size = null;
        protected string[] _groupBy = null;
        protected string[] _etype = null;
        protected string[] _ecolor = null;
        protected string[] _esize = null;
        private double[] _bar = null;

        /// <summary>
        /// 取得或回傳 Type 值 (string[]，值或欄位名稱)
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

        /// <summary>
        /// 取得或回傳 Size 值 (string[]，值或欄位名稱)
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

        /// <summary>
        /// 取得或回傳 Color 值 (string[]，值或欄位名稱)
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

        /// <summary>
        /// 取得或回傳 EType 值 (string[]，值或欄位名稱)
        /// </summary>
        public dynamic EType
        {
            get
            {
                return _etype;
            }
            set
            {
                if (value is string)
                {
                    _etype = new string[] { value };
                }
                else
                {
                    try
                    {
                        _etype = ((int[])MtbTools.ConvertInputToIntArray(value)).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _etype = null;
                    }                    
                }                
            }
        }

        /// <summary>
        /// 取得或回傳 ESize 值 (string[]，值或欄位名稱)
        /// </summary>
        public dynamic ESize
        {
            get
            {
                return _esize;
            }
            set
            {
                
                if (value is string)
                {
                    _esize = new string[] { value };
                }
                else
                {
                    try
                    {
                        _esize = ((int[])MtbTools.ConvertInputToFloatArray(value)).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _esize = null;
                    }                    
                }
            }
        }

        /// <summary>
        /// 取得或回傳 EColor 值 (string[]，值或欄位名稱)
        /// </summary>
        public dynamic EColor
        {
            get
            {
                return _ecolor;
            }
            set
            {
                if (value is string)
                {
                    _ecolor = new string[] { value };
                }
                else
                {
                    try
                    {
                        _ecolor = ((int[])MtbTools.ConvertInputToIntArray(value)).Select(x => x.ToString()).ToArray();
                    }
                    catch
                    {
                        _ecolor = null;
                    }
                }
            }
        }
        
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

        /// <summary>
        /// 設定 Symbol 是否可視
        /// </summary>
        public bool Visible { set; get; }

        /// <summary>
        /// 指定或取得 DataView 的分群變數，可輸入時可一(string)或多個(stirng[])欄位文字，以string[] 回傳
        /// </summary>
        public virtual dynamic GroupingBy
        {
            set
            {
                _groupBy = MtbTools.ConvertInputToStringArray(value);
            }
            get
            {
                return _groupBy;
            }
        }
        
        /// <summary>
        /// 微調的 Datlab 位置與屬性 List
        /// </summary>
        public virtual List<DataViewPosition> DataViewPositionLst { set; get; }

        /// <summary>
        /// 取得 Minitab 指令碼
        /// </summary>
        public Func<string> GetCommand { set; get; }

        /// <summary>
        /// 取得預設的 Minitab 指令碼
        /// </summary>
        /// <returns></returns>
        protected abstract string DefaultCommand();
        public abstract void SetDefault();
        /// <summary>
        /// 複製目前物件的設定
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
    }
}
