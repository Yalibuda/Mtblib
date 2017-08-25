using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component
{
    public abstract class Label : ILabels
    {
        /// <summary>
        /// 回復初始設定值
        /// </summary>
        public abstract void SetDefault();

        /// <summary>
        /// 取得預設的 Minitab 指令
        /// </summary>
        /// <returns></returns>
        protected abstract string DefaultCommand();

        /// <summary>
        /// 取得 Minitab 指令碼
        /// </summary>
        public Func<string> GetCommand { set; get; }

        /// <summary>
        /// 設定或取得 Label 的文字
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// 設定 Label 是否可視
        /// </summary>
        public bool Visible { set; get; }

        protected double[] _offset = null;
        /// <summary>
        /// Moves a title away from the position specified alignment
        /// 輸入一個數值陣列 double[2]，第一個是水平位移(-1~1)、第二個是垂直位移(-1~1)
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

        protected Align _alignment = Align.Center;
        /// <summary>
        /// 指定對齊位置
        /// </summary>
        public Align Alignment { set; get; }

        /// <summary>
        /// 指定或取得字形大小(PSize)
        /// </summary>
        public float FontSize { set; get; }
        /// <summary>
        /// 指定或取得字形顏色
        /// </summary>
        public int FontColor { set; get; }
        /// <summary>
        /// 字體是否粗體
        /// </summary>
        public bool Bold { set; get; }
        /// <summary>
        /// 字體是否加底線
        /// </summary>
        public bool Underline { set; get; }
        /// <summary>
        /// 字體是否斜體
        /// </summary>
        public bool Italic { set; get; }
        /// <summary>
        /// 字體旋轉角度
        /// </summary>
        public double Angle { set; get; }

        /// <summary>
        /// Label 不實作 Clone() 方法，讓繼承的物件自己實作，且不強迫
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
