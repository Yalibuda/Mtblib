using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    public class ContTick : Tick
    {
        public ContTick()
        {
            SetDefault();
        }

        public override void SetDefault()
        {
            NMajor = -1;
            NMinor = -1;
            Increament = MtbTools.MISSINGVALUE;
            FontColor = -1;
            FontSize = -1;
            Bold = false;
            Italic = false;
            Angle = MtbTools.MISSINGVALUE;
            _ticks = null;
            GetCommand = DefaultCommand;
        }
        
        /// <summary>
        /// 設定連續 Scale 的 tick 位置
        /// </summary>
        /// <param name="ticks">可輸入 string (e.g. 欄位名稱 or 已處理過的位置) 或是 string[]</param>
        public override void SetTicks(dynamic ticks = null)
        {
            _ticks = MtbTools.ConvertInputToStringArray(ticks);            
        }

        /// <summary>
        /// 指定 Scale Tick 上要顯示標籤
        /// </summary>
        /// <param name="labels">欄位名稱(string)或是標籤內容(string[])，當輸入標籤內容時，如果是非純數字內容，需於欲顯示的內容前後加上雙引號(")</param>
        public override void SetLabels(dynamic labels = null)
        {            
            _labels = MtbTools.ConvertInputToStringArray(labels);            
        }

        protected override string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (_ticks != null)
            {
                cmnd.AppendLine(" Tick &");
                cmnd.AppendLine(string.Join(" &\r\n", _ticks) + ";");
            }
            if (NMajor > -1) cmnd.AppendLine(string.Format(" Nmajor {0};", NMajor));
            if (NMinor > -1) cmnd.AppendLine(string.Format(" Nminor {0}", NMinor));
            if (_labels != null)
            {
                cmnd.AppendLine(" Label &");
                cmnd.AppendLine(string.Join(" &\r\n", _labels) + ";");
            }            
            if (FontColor > -1) cmnd.AppendLine(string.Format(" Tcolor {0};", FontColor));
            if (FontSize > 0) cmnd.AppendLine(string.Format(" Psize {0};", FontSize));
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Underline;");
            if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format(" Angle {0};", Angle));
            if (HideAllTick)
            {
                cmnd.AppendLine(" TShow;");
            }

            return cmnd.ToString();
        }

        public override object Clone()
        {
            ContTick tick = new ContTick();
            tick.NMajor = this.NMajor;
            tick.NMinor = this.NMinor;
            tick.Increament = this.Increament;
            tick.FontSize = this.FontSize;
            tick.FontColor = this.FontColor;
            tick.Bold = this.Bold;
            tick.Italic = this.Italic;
            tick.Underline = this.Underline;
            tick.Angle = this.Angle;
            tick.SetTicks(this.GetTicks());
            tick.SetLabels(this.GetLabels());
            return tick;
        }
    }
}
