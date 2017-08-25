using Mtb;
using Mtblib.Graph.CategoricalChart;
using Mtblib.Graph.Component;
using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.BarChart
{
    public class Chart : MGraph
    {
        public Chart(Mtb.Project proj, Mtb.Worksheet ws)
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
        /// Chart 的顯示方式列舉，Stack 或是 Cluster
        /// </summary>
        public enum ChartStackType
        {
            Stack, Cluster
        }

        /// <summary>
        /// 輸入單一 Variable 時，可以對數據套用的函數
        /// </summary>
        public enum ChartFunctionType
        {
            SUM, COUNT, N, NMISS, MEAN, MEDIAN, MINIMUM, MAXIMUM, STDEV, SSQ
        }

        /// <summary>
        /// 輸入的 Summarized Data 類型
        /// </summary>
        public enum ChartRepresent
        {
            COUNT_OF_UNIQUE_VALUES, A_FUNCTION_OF_A_VARIABLE, ONE_WAY_TABLE, TWO_WAY_TABLE
        }

        /// <summary>
        /// 以 Two-way table 繪製 Bar chart 時，X軸的選用類型
        /// </summary>
        public enum ChartTableArrangementType
        {
            ///<summary>以列標籤做為分群的最外側</summary>
            RowsOuterMost,
            ///<summary>以行標籤做為分群的最外側</summary>
            ColsOuterMost
        }

        /// <summary>
        /// 設定或取得輸入的用於繪製 Bar Chart 的資料格式?
        /// </summary>
        public ChartRepresent BarsRepresent { set; get; }

        /// <summary>
        /// 指定或取得 Chart 的顯示方式，Stack 或是 Cluster
        /// </summary>
        public ChartStackType StackType { set; get; }
        /// <summary>
        /// 指定或取得 Chart 用於處理數據的函數類型，如果使用 Summarized Data 將無視 FuncType
        /// </summary>
        public ChartFunctionType FuncType { set; get; }
        /// <summary>
        /// 指定或取得 Chart 使用 Two-way table 繪製時，依何種資料(Rows or Cols)做為最外層分群
        /// </summary>
        public ChartTableArrangementType TableArrangement { set; get; }
        /// <summary>
        /// 指定或取得 Chart 是否要顯示 Empty cell
        /// </summary>
        public bool NoEmpty { get; set; }
        /// <summary>
        /// 指定或取得 Chart 是否要顯示 Missing cell
        /// </summary>
        public bool NoMissing { get; set; }

        public Component.Scale.ContScale YScale { set; get; }
        public Component.Scale.CateScale XScale { set; get; }
        public Component.DataView.Bar Bar { set; get; }
        public Component.DataView.Symbol Symbol { set; get; }
        public Component.DataView.Connect Connectline { set; get; }
        public Component.MultiGraph.MPanel Panel { set; get; }
        
        /// <summary>
        /// 設定 X-Y 軸是否對調
        /// </summary>
        public bool Transponse { set; get; }
        
        /// <summary>
        /// 設定或取得 Data label 
        /// </summary>
        public Datlab DataLabel { set; get; }

        /// <summary>
        /// 當顯示 Stack Bar Chart 的 Datlab 時，是否要使用修正的資料標籤?
        /// 即標籤代表各 Stack value，而非 Y 值。
        /// 並且需指定修正過標籤的欄位
        /// </summary>
        public bool AdjDatlabAtStackBar { set; get; }
        
        public override void SetDefault()
        {
            BarsRepresent = Chart.ChartRepresent.A_FUNCTION_OF_A_VARIABLE;
            StackType = Chart.ChartStackType.Cluster;
            FuncType = Chart.ChartFunctionType.SUM;
            TableArrangement = Chart.ChartTableArrangementType.RowsOuterMost;
            XScale = new Component.Scale.CateScale(ScaleDirection.X_Axis);
            YScale = new Component.Scale.ContScale(ScaleDirection.Y_Axis);            
            Bar = new Component.DataView.Bar();
            Symbol = new Component.DataView.Symbol() { Visible = false };
            Connectline = new Component.DataView.Connect() { Visible = false };
            Panel = new Component.MultiGraph.MPanel();
            Transponse = false;
            DataLabel = new Datlab();
            NoEmpty = false;
            NoMissing = false;
            GetCommand = DefaultCommand2;
        }

        protected override string DefaultCommand()
        {
            if (Variables == null) throw new Exception("建立 Bar chart 指令時，未給定 Variables");

            Mtb.Column[] vars = (Mtb.Column[])Variables;
            Mtb.Column[] gps = null;
            if (GroupingVariables != null)
            {
                gps = (Mtb.Column[])GroupingVariables;
            }

            if ((BarsRepresent == ChartRepresent.ONE_WAY_TABLE ||
                BarsRepresent == ChartRepresent.TWO_WAY_TABLE)
                & (gps == null))
            {
                throw new Exception("(Bar chart)使用 Summaried Data，至少給定一個分群欄位");
            }

            StringBuilder cmnd = new StringBuilder(); // local macro 內容

            /*****************
             * 一些注意事項: Chart 可以有多個 Variable 輸入，分成兩類
             * 1. F(C...C)*Cg 這是各別F(C)以 Cg 做分群，不需搭配 Summarized
             * 2. Summarized data，使用 (C...C)*Cg 搭配 Summarized 子命令，
             *    如果是 Two-way table 則須再加上 VFirst 或 VLast 等指令
             * 
             */
            switch (BarsRepresent)
            {
                case ChartRepresent.COUNT_OF_UNIQUE_VALUES:
                    cmnd.AppendFormat("Chart {0};\r\n",
                            string.Join(" &\r\n", vars.Select(x => x.SynthesizedName).ToArray()));
                    if (gps != null)
                    {
                        cmnd.AppendFormat(" Group {0};\r\n",
                        string.Join(" &\r\n", string.Join(" &\r\n", gps.Select(x => x.SynthesizedName).ToArray())));
                    }
                    break;

                case ChartRepresent.A_FUNCTION_OF_A_VARIABLE:
                    cmnd.AppendFormat("Chart {0}({1}) &\r\n",
                        FuncType.ToString(),
                        string.Join(" &\r\n", vars.Select(x => x.SynthesizedName).ToArray()));
                    if (gps != null)
                    {
                        cmnd.AppendFormat("*{0};\r\n", gps[0].SynthesizedName);
                        if (gps.Length >= 2)
                            cmnd.AppendFormat(" Group {0};\r\n",
                            string.Join(" &\r\n", gps.Select((x, i) => new { colId = x.SynthesizedName, index = i }).
                            Where(x => x.index > 0).Select(x => x.colId).ToArray()));
                    }
                    else
                    {
                        cmnd.AppendLine(";");
                    }

                    break;
                case ChartRepresent.ONE_WAY_TABLE:
                case ChartRepresent.TWO_WAY_TABLE:
                    cmnd.AppendFormat("Chart ({0})*{1};\r\n",
                    string.Join(" &\r\n", vars.Select(x => x.SynthesizedName).ToArray()),
                    gps[0].SynthesizedName);
                    cmnd.AppendLine("Summarized;");
                    if (BarsRepresent == ChartRepresent.TWO_WAY_TABLE)
                    {
                        cmnd.AppendLine("Overlay;");
                        if (TableArrangement == ChartTableArrangementType.RowsOuterMost)
                        {
                            cmnd.AppendLine(" VLast;");
                        }
                        else
                        {
                            cmnd.AppendLine(" VFirst;");
                        }
                    }
                    if (gps.Length >= 2)
                    {
                        cmnd.AppendFormat(" Group {0};\r\n",
                           string.Join(" &\r\n", gps.Select((x, i) => new { colId = x.SynthesizedName, index = i }).
                           Where(x => x.index > 0).Select(x => x.colId).ToArray()));
                    }
                    break;

            }
            cmnd.Append(GetOptionCommand());
            cmnd.Append(YScale.GetCommand());
            cmnd.Append(XScale.GetCommand());

            //
            if (StackType == ChartStackType.Stack && DataLabel.Visible && AdjDatlabAtStackBar)
            {

            }
            cmnd.Append(DataLabel.GetCommand());



            cmnd.Append(Bar.GetCommand());
            cmnd.Append(Symbol.GetCommand());
            cmnd.Append(Connectline.GetCommand());
            cmnd.Append(Panel.GetCommand());
            cmnd.Append(Legend.GetCommand());

            cmnd.Append(GetAnnotationCommand());
            cmnd.Append(GetRegionCommand());


            return cmnd.ToString() + ".";

        }

        private string DefaultCommand2()
        {
            if (Variables == null) throw new Exception("建立 Bar chart 指令時，未給定 Variables");

            Mtb.Column[] vars = (Mtb.Column[])Variables;
            Mtb.Column[] gps = null;
            if (GroupingVariables != null)
            {
                gps = (Mtb.Column[])GroupingVariables;
            }

            switch (BarsRepresent)// 以一次畫一張圖為目的，過濾掉不合法的輸入數量
            {
                case ChartRepresent.COUNT_OF_UNIQUE_VALUES:
                case ChartRepresent.A_FUNCTION_OF_A_VARIABLE:
                case ChartRepresent.ONE_WAY_TABLE:
                    if (vars.Length > 1) throw new ArgumentException(
                        string.Format("BarsRepresent={0}時，不支援同時繪製多變數", BarsRepresent.ToString()));
                    break;
            }
            switch (BarsRepresent)// 判斷群組數是否合法
            {
                case ChartRepresent.ONE_WAY_TABLE:
                case ChartRepresent.TWO_WAY_TABLE:
                    if (gps == null) throw new Exception("(Bar chart)使用 Summaried Data，至少給定一個分群欄位");
                    break;
            }


            StringBuilder cmnd = new StringBuilder(); // local macro 內容

            cmnd.AppendLine("macro");
            cmnd.AppendLine("chart y.1-y.n;");
            cmnd.AppendLine("group x.1-x.m;");
            cmnd.AppendLine("pane p.1-p.k;"); //如果使用者有指定 panel 
            cmnd.AppendLine("datlab dlab."); //如果使用者有自己指定 column for datlab

            cmnd.AppendLine("mcolumn y.1-y.n");
            cmnd.AppendLine("mcolumn x.1-x.m");
            cmnd.AppendLine("mcolumn p.1-p.k");
            cmnd.AppendLine("mcolumn yy ylab stkdlab dlab xx.1-xx.m");
            cmnd.AppendLine("mconstant nn");

            //準備使用於圖中的額外資料
            Datlab tmpDatlab = (Datlab)DataLabel.Clone();
            if (StackType == ChartStackType.Stack && DataLabel.Visible && AdjDatlabAtStackBar)
            {
                #region 建立 Adjust stack bar chart 要的 datlab
                switch (BarsRepresent)
                {
                    case ChartRepresent.COUNT_OF_UNIQUE_VALUES:
                        cmnd.AppendLine("count y.1 nn");
                        cmnd.AppendLine("set yy");
                        cmnd.AppendLine("(1)nn");
                        cmnd.AppendLine("end");
                        cmnd.AppendLine("stat yy;");
                        cmnd.AppendLine("by y.1 x.1-x.m;");
                        cmnd.AppendLine("sums stkdlab.");                        
                        break;
                    case ChartRepresent.A_FUNCTION_OF_A_VARIABLE:
                        cmnd.AppendLine("stat y.1;");
                        cmnd.AppendLine("by x.1-x.m;");
                        cmnd.AppendFormat("{0} stkdlab.\r\n",
                            FuncType.ToString().ToLower() == "sum" ? "sums" : FuncType.ToString().ToLower());
                        break;
                    case ChartRepresent.ONE_WAY_TABLE:
                        cmnd.AppendLine("copy y.1 stkdlab");
                        break;
                    case ChartRepresent.TWO_WAY_TABLE:
                        cmnd.AppendLine("stack y.1-y.n yy.");
                        cmnd.AppendLine("count y.1 nn");
                        cmnd.AppendLine("tset ylab");
                        cmnd.AppendFormat("1({0})nn\r\n",
                            string.Join(" &\r\n", vars.Select(x => "\"" + x.Name + "\"").ToArray()));
                        cmnd.AppendLine("end");
                        List<string[]> gpData = new List<string[]>();
                        for (int i = 0; i < gps.Length; i++)
                        {
                            Mtb.Column col = gps[i];
                            string[] data;
                            switch (col.DataType)
                            {
                                case MtbDataTypes.DataUnassigned:
                                    throw new ArgumentNullException("輸入的欄位沒有資料");
                                default:
                                case MtbDataTypes.DateTime:
                                case MtbDataTypes.Numeric:
                                    data = ((double[])col.GetData()).Select(x => x.ToString()).ToArray();
                                    break;
                                case MtbDataTypes.Text:
                                    data = col.GetData();
                                    break;
                            }
                            cmnd.AppendFormat("tset xx.{0}\r\n", i + 1);
                            cmnd.AppendFormat("{1}({0})1\r\n",
                                string.Join(" &\r\n", data.Select(x => "\"" + x + "\"").ToArray()),
                                vars.Length);
                            cmnd.AppendLine("end");
                            cmnd.AppendLine("vorder ylab xx.1-xx.m;");
                            cmnd.AppendLine("work.");
                            cmnd.AppendLine("stat yy;");
                            cmnd.AppendLine("by ylab xx.1-xx.m;");
                            cmnd.AppendLine("sums stkdlab.");
                        }
                        break;
                    default:
                        break;
                }
                cmnd.AppendLine("text stkdlab stkdlab");
                #endregion

                tmpDatlab.LabelColumn = "stkdlab";
                tmpDatlab.Placement = new double[] { 0, -1 };
            }
            else
            {
                if (DataLabel.LabelColumn != null) tmpDatlab.LabelColumn = "dlab";
            }

            switch (BarsRepresent)
            {
                case ChartRepresent.COUNT_OF_UNIQUE_VALUES:
                    cmnd.AppendLine("Chart y.1-y.n;");
                    if (gps != null)
                    {
                        cmnd.AppendLine("Group x.1-x.n;");
                    }
                    break;

                case ChartRepresent.A_FUNCTION_OF_A_VARIABLE:
                    cmnd.AppendFormat("Chart {0}(y.1-y.n) &\r\n",
                        FuncType.ToString());
                    if (gps != null)
                    {
                        cmnd.AppendLine("*x.1;");
                        if (gps.Length >= 2) cmnd.AppendLine(" Group x.2-x.m;");
                    }
                    else
                    {
                        cmnd.AppendLine(";");
                    }

                    break;
                case ChartRepresent.ONE_WAY_TABLE:
                case ChartRepresent.TWO_WAY_TABLE:
                    cmnd.AppendLine("Chart (y.1-y.n)*x.1;");
                    cmnd.AppendLine("Summarized;");
                    if (BarsRepresent == ChartRepresent.TWO_WAY_TABLE)
                    {
                        cmnd.AppendLine("Overlay;");
                        if (TableArrangement == ChartTableArrangementType.RowsOuterMost)
                        {
                            cmnd.AppendLine(" VLast;");
                        }
                        else
                        {
                            cmnd.AppendLine(" VFirst;");
                        }
                    }
                    if (gps.Length >= 2) cmnd.AppendLine(" Group x.2-x.m;");
                    break;
            }
            if (Transponse) cmnd.AppendLine("trans;");
            if (StackType == ChartStackType.Stack) cmnd.AppendLine("stack;");
            cmnd.Append(GetOptionCommand());
            cmnd.Append(YScale.GetCommand());            
            cmnd.Append(XScale.GetCommand());
            cmnd.Append(tmpDatlab.GetCommand());

            /*
             * 對每一個 DataView 建立 Command
             * 這些處理是為了將 GroupingBy 屬性由原欄位換成 macro coded name
             */
            Component.DataView.DataView tmpDataview;
            string[] xStr = gps.Select((x, i) => "x." + (i + 1)).ToArray();

            foreach (Component.DataView.DataView dview in
                new Component.DataView.DataView[] { Bar, Symbol, Connectline })
            {
                tmpDataview = (Component.DataView.DataView)dview.Clone();
                if (dview.GroupingBy != null)
                {
                    string[] g = MtbTools.ConvertToMacroCodedName(
                        (string[])tmpDataview.GroupingBy, gps, xStr, _ws);
                }
                //if (dview is Component.DataView.Bar)
                //{
                //    if(Bar.AssignAttributeByVariables)
                //}
                cmnd.Append(tmpDataview.GetCommand());
            }

            Component.MultiGraph.MPanel tmpPane = (Component.MultiGraph.MPanel)Panel.Clone();
            if (Panel.PaneledBy != null)
            {
                tmpPane.PaneledBy = "p.1-p.k";
            }
            cmnd.Append(tmpPane.GetCommand());
            cmnd.Append(Legend.GetCommand());
            if (NoMissing) cmnd.AppendLine("nomiss;");
            if (NoEmpty) cmnd.AppendLine("noem;");
            cmnd.Append(GetAnnotationCommand());
            cmnd.Append(GetRegionCommand());
            cmnd.AppendLine(".");
            cmnd.AppendLine("endmacro");
            return cmnd.ToString();



        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            Variables = null;
            GroupingVariables = null;
            base.Dispose(disposing);
        }
        ~Chart()
        {
            Dispose(false);
        }
    }
}
