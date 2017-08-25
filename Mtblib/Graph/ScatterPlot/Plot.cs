using Mtb;
using Mtblib.Graph.CategoricalChart;
using Mtblib.Graph.Component;
using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mtblib.Graph.ScatterPlot
{
    public class Plot : MGraph
    {
        public Plot(Mtb.Project proj, Mtb.Worksheet ws)
            : base(proj, ws)
        {
            SetDefault();
        }

        private Mtb.Column[] _yvariables = null;
        /// <summary>
        /// 指定或取得要繪製的欄位，合法的 Set 為一(string/Mtb.Column)或多個(string[]/Mtb.Column[])欄位，
        /// 也可使用連續輸入表示式(string)，如: C1-C3，可用單引號名稱或是 Column id。使用 Get 取得 Minitab
        /// 欄位陣列(Mtb.Column[])
        /// </summary>
        public dynamic YVariables
        {
            set
            {
                if (value == null)
                {
                    _yvariables = null;
                }
                else
                {
                    _yvariables = MtbTools.GetMatchColumns(value, _ws);
                }

            }
            get
            {
                return _yvariables;
            }
        }

        private Mtb.Column[] _xvariables = null;
        /// <summary>
        /// 指定或取得要繪製的欄位，合法的 Set 為一(string/Mtb.Column)或多個(string[]/Mtb.Column[])欄位，
        /// 也可使用連續輸入表示式(string)，如: C1-C3，可用單引號名稱或是 Column id。使用 Get 取得 Minitab
        /// 欄位陣列(Mtb.Column[])
        /// </summary>
        public dynamic XVariables
        {
            set
            {
                if (value == null)
                {
                    _xvariables = null;
                }
                else
                {
                    _xvariables = MtbTools.GetMatchColumns(value, _ws);
                }

            }
            get
            {
                return _xvariables;
            }
        }

        Mtb.Column[] _grouping = null;
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

        /// <summary>
        /// 多變數輸入時，建立 Plot 的方式
        /// </summary>
        public enum MultipleGraphType
        {
            ///<summary>一對一繪圖，X與Y的數量相同時</summary>
            Regular = 0,
            ///<summary>XY變數任二個繪圖，X與Y的數量不需相同</summary>
            Expanded = 1,
            ///<summary>將所有變數合併一起繪圖，X與Y的數量不需相同</summary>
            Overlay = 2
        }

        /// <summary>
        /// 設定或取得多變數輸入時，建立 Plot 指令的方式
        /// </summary>
        public MultipleGraphType GraphType { set; get; }
        public Component.Scale.ContScale YScale { set; get; }
        public Component.Scale.ContScale XScale { set; get; }
        public Component.DataView.Symbol Symbol { set; get; }
        public Component.DataView.Connect Connectline { set; get; }
        public Component.DataView.Projection Projection { set; get; }
        public Datlab DataLabel { set; get; }
        public Component.MultiGraph.MPanel Panel { set; get; }

        /// <summary>
        /// 回復成預設狀態
        /// </summary>
        public override void SetDefault()
        {
            YVariables = null;
            XVariables = null;
            GroupingVariables = null;
            GraphType = MultipleGraphType.Regular;
            YScale = new Component.Scale.ContScale(ScaleDirection.Y_Axis);           
            XScale = new Component.Scale.ContScale(ScaleDirection.X_Axis);           
            Symbol = new Component.DataView.Symbol() { Visible = true };
            Connectline = new Component.DataView.Connect() { Visible = false };
            Projection = new Component.DataView.Projection() { Visible = false };
            Panel = new Component.MultiGraph.MPanel();
            DataLabel = new Datlab();
            GetCommand = DefaultCommand;
        }

        /// <summary>
        /// 預設指令碼
        /// </summary>
        /// <returns></returns>
        protected override string DefaultCommand()
        {
            if (YVariables == null) throw new Exception("建立 Plot 指令時，未給定 YVariables");
            if (XVariables == null) throw new Exception("建立 Plot 指令時，未給定 XVariables");


            Mtb.Column[] yvars = (Mtb.Column[])YVariables;
            Mtb.Column[] xvars = (Mtb.Column[])XVariables;

            Mtb.Column[] gps = null;
            if (GroupingVariables != null)
            {
                gps = (Mtb.Column[])GroupingVariables;
            }


            StringBuilder cmnd = new StringBuilder();

            if (GraphType == MultipleGraphType.Regular)
            {
                if (yvars.Length != xvars.Length) throw new ArgumentException("GraphType=Regular 時，X 變數和 Y 變數的數量不同");
                cmnd.AppendLine("Plot &");
                for (int i = 0; i < yvars.Length; i++)
                {
                    cmnd.AppendFormat("{0}*{1} &\r\n", yvars[i].SynthesizedName, xvars[i].SynthesizedName);
                }
                cmnd.AppendLine(";");
            }
            else
            {
                cmnd.AppendFormat("Plot ({0})*({1});\r\n",
                    string.Join(" &\r\n", yvars.Select(x => x.SynthesizedName).ToArray()),
                    string.Join(" &\r\n", xvars.Select(x => x.SynthesizedName).ToArray()));
                if (GraphType == MultipleGraphType.Overlay)
                {
                    cmnd.AppendLine(" Over;");
                }
            }

            if (gps != null)
            {
                string[] gp = gps.Select(x=>x.SynthesizedName).ToArray();
                Symbol.GroupingBy = gp.Clone();
                Connectline.GroupingBy = gp.Clone();
                Projection.GroupingBy = gp.Clone();
            }

            cmnd.AppendFormat("");

            cmnd.Append(GetOptionCommand());
            cmnd.Append(YScale.GetCommand());          
            cmnd.Append(XScale.GetCommand());           
            cmnd.Append(Symbol.GetCommand());
            cmnd.Append(Connectline.GetCommand());
            cmnd.Append(Projection.GetCommand());
            cmnd.Append(Panel.GetCommand());

            cmnd.Append(GetAnnotationCommand());
            cmnd.Append(GetRegionCommand());


            return cmnd.ToString() + ".";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            _yvariables = null;
            _xvariables = null;
            _grouping = null;
            base.Dispose(disposing);
        }
        ~Plot()
        {
            Dispose(false);
        }
    }
}
