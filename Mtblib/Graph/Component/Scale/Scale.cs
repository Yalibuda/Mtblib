using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    /// <summary>
    /// Minitab 座標軸的抽象介面
    /// </summary>
    public abstract class Scale : IScale
    {
        //protected ScaleDirection _scaleDirection;
        //protected ScalePrimary _scalePrimary;
        //protected ScaleType _scaleType;

        public Scale(ScaleDirection scaleDirection)
        {
            Direction = scaleDirection;
            SetDefault();
        }

        /// <summary>
        /// Scale 的下限
        /// </summary>
        public double Min { set; get; }
        /// <summary>
        /// Scale 的上限
        /// </summary>
        public double Max { set; get; }
        /// <summary>
        /// Scale 上 Tick 的屬性
        /// </summary>
        public Tick Ticks { set; get; }

        /// <summary>
        /// Scale 上 Refe 屬性
        /// </summary>
        public Refe Refes { set; get; }

        /// <summary>
        /// Scale label 的屬性
        /// </summary>
        public AxLabel Label { set; get; }

        /// <summary>
        /// 將參數設置為預設值
        /// </summary>
        public abstract void SetDefault();

        /// <summary>
        /// 取得預設的 Scale 指令(需實作)
        /// </summary>
        /// <returns></returns>
        protected abstract string DefaultCommand();

        /// <summary>
        /// 取得 Scale 指令
        /// </summary>
        public Func<string> GetCommand { set; get; }

        protected int[] _lDisplay;
        /// <summary>
        /// 設定或取得 Low side 的 Scale 元件顯示方式，合法的輸入方式為4個(0 或 1)的整數陣列 int[]，依序為
        /// Axline, MajorTick, MajorTickLab, MinorTick；其中0=不顯示，1=顯示
        /// </summary>
        public int[] LDisplay
        {
            set
            {
                if (value != null && value.Length != 4)
                {
                    throw new ArgumentException("LDisply 的參數數需等於4");
                }
                if (value != null && value.Count(x => x != 1 && x != 0) > 0)
                {
                    throw new ArgumentOutOfRangeException("LDisplay 的參數值只能是0或1");
                }
                _lDisplay = MtbTools.ConvertInputToIntArray(value);
            }
            get
            {
                return _lDisplay;

            }
        }

        protected int[] _hDisplay;
        /// <summary>
        /// 設定或取得 High side 的 Scale 元件顯示方式，合法的輸入方式為4個(0 或 1)的整數陣列 int[]，依序為
        /// Axline, MajorTick, MajorTickLab, MinorTick；其中0=不顯示，1=顯示
        /// </summary>
        public int[] HDisplay
        {
            set
            {
                if (value != null && value.Length != 4)
                {
                    throw new ArgumentException("HDisply 的參數數需等於4");
                }
                if (value != null && value.Count(x => x != 1 && x != 0) > 0)
                {
                    throw new ArgumentOutOfRangeException("HDisplay 的參數值只能是0或1");
                }
                _hDisplay = MtbTools.ConvertInputToIntArray(value);
            }
            get
            {
                return _hDisplay;
            }
        }

        /// <summary>
        /// 指定或取得 Scale 的方向
        /// </summary>
        public ScaleDirection Direction { set; get; }
        public abstract object Clone();

        ///// <summary>
        ///// 設定 Low side 的 Scale元件是否要顯示
        ///// </summary>
        ///// <param name="axisline"></param>
        ///// <param name="majortick"></param>
        ///// <param name="majorticklab"></param>
        ///// <param name="minortick"></param>
        //public void SetLDisplay(int axisline = 1, int majortick = 1, int majorticklab = 1, int minortick = 0)
        //{
        //    _lDisplay = new int[] { axisline, majortick, majorticklab, minortick };
        //}
        ///// <summary>
        ///// 取得 Low side 的 Scale 元件的顯示設定
        ///// </summary>
        ///// <returns></returns>
        //public int[] GetLDisplay()
        //{
        //    return _lDisplay;
        //}

        ///// <summary>
        ///// 設定 High side 的 Scale元件是否要顯示
        ///// </summary>
        ///// <param name="axisline"></param>
        ///// <param name="majortick"></param>
        ///// <param name="majorticklab"></param>
        ///// <param name="minortick"></param>
        //public void SetHDisplay(int axisline = 1, int majortick = 0, int majorticklab = 0, int minortick = 0)
        //{
        //    _hDisplay = new int[] { axisline, majortick, majorticklab, minortick };
        //}

        ///// <summary>
        ///// 取得 High side 的 Scale 元件的顯示設定
        ///// </summary>
        ///// <returns></returns>
        //public int[] GetHDisplay()
        //{
        //    return _hDisplay;
        //}

    }
}
