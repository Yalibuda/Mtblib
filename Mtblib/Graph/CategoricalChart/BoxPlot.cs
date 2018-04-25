using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;
using Mtblib.Graph.Component;
using Mtblib.Graph.Component.Scale;
using Mtblib.Graph.Component.MultiGraph;

namespace Mtblib.Graph.CategoricalChart
{
    /// <summary>
    /// 處理的資料以
    /// </summary>
    public class BoxPlot : MGraph
    {

        public BoxPlot(Mtb.Project proj, Mtb.Worksheet ws)
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

        public Mean Mean { set; get; }
        public CMean CMean { set; get; }
        public IQRBox IQRBox { set; get; }
        public RangeBox RBox { set; get; }
        public Outlier Outlier { set; get; }
        public Individual Individual { set; get; }
        public Whisker Whisker { set; get; }

        public ContScale YScale { set; get; }
        public CateScale XScale { set; get; }
        public MPanel Panel { set; get; }
        public Meanlab MeanDatlab { set; get; }
        public Indivlab IndivDatlab { set; get; }

        public override void SetDefault()
        {
            Variables = null;
            GroupingVariables = null;
            Mean = new Mean() { Visible = false };
            CMean = new CMean() { Visible = false };
            IQRBox = new IQRBox() { Visible = true };
            RBox = new RangeBox() { Visible = false };
            Outlier = new Outlier() { Visible = true };
            Individual = new Individual() { Visible = false };
            Whisker = new Whisker() { Visible = true };
            YScale = new ContScale(ScaleDirection.Y_Axis);
            XScale = new CateScale(ScaleDirection.X_Axis);          
            Panel = new MPanel();
            MeanDatlab = new Meanlab();
            IndivDatlab = new Indivlab();
            //MeanDatlab.GetCommand = () =>
            //{
            //    if (!MeanDatlab.Visible) return "";
            //    if (MeanDatlab.DatlabType == Component.Datlab.DisplayType.Column && MeanDatlab.LabelColumn == null) return "# 未指定給 Datlab 欄位\r\n";
            //    StringBuilder cmnd = new StringBuilder();
            //    switch (MeanDatlab.DatlabType)
            //    {
            //        case Component.Datlab.DisplayType.YValue:
            //            cmnd.AppendLine(" Mealab;");
            //            cmnd.AppendLine("  YValue;");
            //            break;
            //        case Component.Datlab.DisplayType.RowNumber:
            //            cmnd.AppendLine(" Mealab;");
            //            cmnd.AppendLine("  Rownum;");
            //            break;
            //        case Component.Datlab.DisplayType.Column:
            //            cmnd.AppendFormat(" Mealab {0};\r\n", MeanDatlab.LabelColumn);
            //            break;
            //    }
            //    if (MeanDatlab.FontColor > 0) cmnd.AppendFormat("  TColor {0};\r\n", MeanDatlab.FontColor);
            //    if (MeanDatlab.FontSize > 0) cmnd.AppendFormat("  PSize {0};\r\n", MeanDatlab.FontSize);
            //    if (MeanDatlab.Bold) cmnd.AppendLine("  Bold;");
            //    if (MeanDatlab.Italic) cmnd.AppendLine("  Italic;");
            //    if (MeanDatlab.Underline) cmnd.AppendLine("  Underline;");
            //    if (MeanDatlab.Angle < MtbTools.MISSINGVALUE) cmnd.AppendFormat("  Angle {0};\r\n", MeanDatlab.Angle);
            //    if (MeanDatlab.Offset != null) cmnd.AppendFormat("  Offset {0};\r\n", string.Join(" &\r\n", MeanDatlab.Offset));
            //    if (MeanDatlab.Placement != null) cmnd.AppendFormat("  Placement {0};\r\n", string.Join(" &\r\n", MeanDatlab.Placement));

            //    foreach (LabelPosition pos in MeanDatlab.PositionList)
            //    {
            //        cmnd.Append(pos.GetCommand());
            //    }
            //    cmnd.Append(Legend.GetCommand());
            //    return cmnd.ToString();
            //};
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (Variables == null) return "";

            Mtb.Column[] vars = (Mtb.Column[])Variables;
            Mtb.Column[] gps = null;
            if (GroupingVariables != null)
            {
                gps = (Mtb.Column[])GroupingVariables;
            }

            StringBuilder cmnd = new StringBuilder(); // local macro 內容
            if (gps != null)
            {
                cmnd.AppendFormat("Boxplot ({0})*{1};\r\n",
                    string.Join(" &\r\n", vars.Select(x => x.SynthesizedName).ToArray()),
                    gps[0].SynthesizedName);
                if (gps.Length >= 2)
                    cmnd.AppendFormat(" Group {0};\r\n",
                    string.Join(" &\r\n", gps.Select((x, i) => new { colId = x.SynthesizedName, index = i }).
                    Where(x => x.index > 0).Select(x => x.colId).ToArray()));
            }
            else
            {
                cmnd.AppendFormat("Boxplot {0};\r\n",
                    string.Join(" &\r\n", vars.Select(x => x.SynthesizedName).ToArray()));
            }
            cmnd.Append(GetOptionCommand());

            cmnd.Append(YScale.GetCommand());
            cmnd.Append(XScale.GetCommand());         

            cmnd.Append(Mean.GetCommand());
            cmnd.Append(CMean.GetCommand());
            cmnd.Append(RBox.GetCommand());
            cmnd.Append(IQRBox.GetCommand());
            cmnd.Append(Whisker.GetCommand());
            cmnd.Append(Outlier.GetCommand());
            cmnd.Append(Individual.GetCommand());
            cmnd.Append(MeanDatlab.GetCommand());
            cmnd.Append(IndivDatlab.GetCommand());

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
            _variables = null;
            _grouping = null;
            base.Dispose(disposing);
        }
        ~BoxPlot()
        {
            Dispose(false);
        }
    }
}
