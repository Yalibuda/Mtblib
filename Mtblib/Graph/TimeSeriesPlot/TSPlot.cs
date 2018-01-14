using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Graph.Component;
using Mtblib.Tools;

namespace Mtblib.Graph.TimeSeriesPlot
{
    public class TSPlot : MGraph
    {
        /// <summary>
        /// TSPlot 適用於 Unstack data 的 Time Serise Plot
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="ws"></param>
        public TSPlot(Mtb.Project proj, Mtb.Worksheet ws)
            : base(proj, ws)
        {
            SetDefault();
        }

        private Mtb.Column[] _variables = null;
        /// <summary>
        /// 指定或取得要繪製的欄位，合法的 Set 為一(string/Mtb.Column)或多個(string[]/Mtb.Column[])欄位，
        /// 也可使用連續輸入表示式(string)，如: C1-C3，可用單引號名稱或是 Column id。使用 Get 取得 Minitab
        /// 欄位陣列(Mtb.Column[])
        /// </summary>
        public dynamic Variables
        {
            set
            {
                if (value == null)
                {
                    _variables = null;
                }
                else
                {
                    _variables = MtbTools.GetMatchColumns(value, _ws);
                }

            }
            get
            {
                return _variables;
            }
        }

        private Mtb.Column[] _grouping = null;
        /// <summary>
        /// 指定或取得要分群的欄位，合法的 Set 為一(string/Mtb.Column)或多個(string[]/Mtb.Column[])欄位，
        /// 最多4組，也可使用連續輸入表示式(string)，如: C1-C3，可用單引號名稱或是 Column id。使用 Get 取
        /// 得 Minitab 欄位陣列(Mtb.Column[])
        /// </summary>
        public dynamic GroupingVariables
        {
            set
            {
                if (value == null)
                {
                    _grouping = null;
                }
                else
                {
                    _grouping = MtbTools.GetMatchColumns(value, _ws);
                }
            }
            get
            {
                return _grouping;
            }
        }

        private Mtb.Column[] _stamps = null;
        /// <summary>
        /// 指定或取得要時間序列圖 Stamp 的欄位，合法的 Set 為一(string/Mtb.Column)或多個(string[]/Mtb.Column[])欄位，
        /// 最多3組，也可使用連續輸入表示式(string)，如: C1-C3，可用單引號名稱或是 Column id。使用 Get 取
        /// 得 Minitab 欄位陣列(Mtb.Column[])
        /// </summary>
        public dynamic Stamp
        {
            get { return _stamps; }
            set
            {
                if (value == null)
                {
                    _stamps = null;
                }
                else
                {
                    _stamps = MtbTools.GetMatchColumns(value, _ws);
                }
            }
        }


        public override void SetDefault()
        {
            XScale = new Component.Scale.ContScale(ScaleDirection.X_Axis);
            YScale = new Component.Scale.ContScale(ScaleDirection.Y_Axis);
            Symbol = new Component.DataView.Symbol() { Visible = false };
            Connectline = new Component.DataView.Connect() { Visible = false };
            Panel = new Component.MultiGraph.MPanel();
            DataLabel = new Datlab();
            NoEmpty = false;
            NoMissing = false;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            throw new NotImplementedException();
            
        }

        public Component.Scale.ContScale YScale { set; get; }
        public Component.Scale.ContScale XScale { set; get; }
        public Component.DataView.Symbol Symbol { set; get; }
        public Component.DataView.Connect Connectline { set; get; }
        public Component.MultiGraph.MPanel Panel { set; get; }

        /// <summary>
        /// 設定或取得 Data label 
        /// </summary>
        public Datlab DataLabel { set; get; }
        // <summary>
        /// 指定或取得 TSPLOT 是否要顯示沒有對應到值的 Group 組合
        /// </summary>
        public bool NoEmpty { get; set; }
        /// <summary>
        /// 指定或取得 TSPLOT 是否要顯示包含 Missing level 的 Group 組合 
        /// </summary>
        public bool NoMissing { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            Variables = null;
            GroupingVariables = null;
            base.Dispose(disposing);
        }
        ~TSPlot()
        {
            Dispose(false);
        }

    }
}
