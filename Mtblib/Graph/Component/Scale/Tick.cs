using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    /// <summary>
    /// 連續型圖形的 Tick
    /// </summary>
    public abstract class Tick : ITick
    {

        public int Start { set; get; }

        /// <summary>
        /// <para>刻度的間距</para>
        /// <para>在連續型刻度中沒有作用，但是提供在介面中保留彈性(例如: 給定間距生成特定刻度組，可在計算 NMAJOR 後回補得到接近的結果)</para>
        /// </summary>
        public virtual double Increament { set; get; }

        public int Level { set; get; }

        public int NMajor { set; get; }

        public int NMinor { set; get; }

        public float FontSize { set; get; }

        public int FontColor { set; get; }

        public bool Bold { set; get; }

        public bool Underline { set; get; }

        public bool Italic { set; get; }

        public double Angle { set; get; }

        public double[] Offset { set; get; }

        public double[] Placement { set; get; }

        protected int[] _tshow = null;
        /// <summary>
        /// 多水準 Tick 時指定或取得要顯示的 Tick 水準(int or int[])
        /// </summary>
        public dynamic TShow
        {
            set
            {
                if (value is Array && value.Length>4)
                {
                    throw new ArgumentException("TShow 只能輸入長度 1~4 的陣列");
                }
                _tshow = MtbTools.ConvertInputToIntArray(value);
            }
            get
            {
                return _tshow;
            }
        }
        /// <summary>
        /// Tick label 是否全部隱藏? 
        /// </summary>
        public bool HideAllTick { set; get; }

        protected string[] _ticks = null;
        /// <summary>
        /// 設置連續型 Scale 要顯示的 Tick 位置
        /// </summary>
        /// <param name="ticks">代表位置的文字陣列</param>
        public abstract void SetTicks(dynamic ticks);

        /// <summary>
        /// 取得連續型 Scale 要顯示的 Tick 位置
        /// </summary>
        /// <returns></returns>
        public string[] GetTicks()
        {
            return _ticks;
        }

        protected string[] _labels = null;
        /// <summary>
        /// 指定 Scale Tick 上要顯示標籤
        /// </summary>
        /// <param name="labels">欄位名稱(string)或是標籤內容(string[])，當輸入標籤內容時，如果是非純數字內容，需於欲顯示的內容前後加上雙引號(")</param>
        public abstract void SetLabels(dynamic labels);

        /// <summary>
        /// 取得 Scale Tick 上要顯示標籤
        /// </summary>
        /// <returns></returns>
        public string[] GetLabels()
        {
            return _labels;
        }

        /// <summary>
        /// 將 Tick 屬性設置成預設值
        /// </summary>
        public abstract void SetDefault();

        /// <summary>
        /// 取得預設的 Tick 指令碼
        /// </summary>
        /// <returns></returns>
        protected abstract string DefaultCommand();

        /// <summary>
        /// 取得 Tick 的指令碼
        /// </summary>
        public Func<string> GetCommand { set; get; }

        /// <summary>
        /// 複製 Tick 的屬性
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
    }
}
