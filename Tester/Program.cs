using Mtblib.Graph.CategoricalChart;
using Mtblib.Graph.Component;
using Mtblib.Graph.Component.Scale;
using Mtblib.Graph.BarChart;
using Mtblib.Graph.ScatterPlot;
using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet;
using LinearAlgebra = MathNet.Numerics.LinearAlgebra;


namespace Tester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Mtb.Application mtbApp = new Mtb.Application();
            mtbApp.UserInterface.DisplayAlerts = false;
            mtbApp.UserInterface.Visible = true;
            Mtb.Project proj = mtbApp.ActiveProject;
            proj.Worksheets.Open(@"D:\Dropbox\Workspace\01.chipMOS\06.Dataset\Defect Ratio by Lot by Customer.MTW");
            Mtb.Worksheet ws = proj.ActiveWorksheet;
            double sstdev = ((double[])ws.Columns.Item("C10").GetData()).StdDev();
            Console.Write(sstdev);
            Chart barchart = new Chart(proj,ws);
            barchart.Variables = "C10 C11";
            barchart.BarsRepresent = Chart.ChartRepresent.TWO_WAY_TABLE;
            barchart.GroupingVariables = "C2 C3";
            barchart.Bar.GroupingBy = "C2 C3";
            barchart.NoMissing = true;
            barchart.NoEmpty = true;
            string path2 = MtbTools.BuildTemporaryMacro("mycode.mtb", barchart.GetCommand());
            proj.ExecuteCommand(string.Format("% \"{0}\" 1", path2));



            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\Test\test20160611.mtw");
            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\03.PFG\06.Dataset\熔爐資料\FurnWall.mtw");
            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\Test\test20160810.mtw");
            //ws = proj.ActiveWorksheet;
            //Mtb.Column[] cols = new Mtb.Column[] { 
            //    ws.Columns.Item("C1"), 
            //    ws.Columns.Item("C2"), 
            //    ws.Columns.Item("C3"), 
            //    ws.Columns.Item("C4"),
            //    ws.Columns.Item("C5"),
            //    ws.Columns.Item("C6")
            //};

            //ws.Columns.Add().Name="xxx";
            //ws.Columns.Item("xxx").SetData("a", 1, 10);
            //System.Data.DataTable dt = Mtblib.Tools.MtbTools.GetDataTableFromMtbCols(cols);
            //System.Data.DataTable result = Mtblib.Tools.MtbTools.Apply(
            //    "C3", Mtblib.Tools.Arithmetic.NMiss, new string[] { "C1", "C2" }, dt);

            


            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\03.PFG\06.Dataset\熔爐資料\FurnWall.mtw");
            //ws = proj.ActiveWorksheet;
            //double[] data = ws.Columns.Item("C5").GetData();
            //string[] name = ws.Columns.Item("C3").GetData();
            //DateTime[] timeorder = ws.Columns.Item("C4").GetData();
            //var data1 = data.Zip(name, (x, y) => new { Value = x, Name = y });
            //var datas = data1.Zip(timeorder, (x, y) => new { Value = x.Value, Name = x.Name, Time = y }).ToArray();
            //string num = "17";
            //var subdata = datas.Where(x => x.Name == num).Select(x => x.Value).ToArray();


            //Mtblib.Stat.ChangePoints.ChangePointInfo[] changePts = Mtblib.Stat.ChangePoints.Run(subdata);
            //System.Text.StringBuilder cmnd = new System.Text.StringBuilder();
            //cmnd.AppendLine("plot c5*c4;");
            //cmnd.AppendLine("symb;");
            //cmnd.AppendLine("conn;");
            //cmnd.AppendLine("Include;");
            //cmnd.AppendFormat("where \"c3=\"\"{0}\"\" \";\r\n",num);            
            //dynamic xvalue = datas.Where(x => x.Name == num).Select(x => x.Time).ToArray();
            //double[] dxvalue;
            //if (xvalue is DateTime[])
            //{
            //    dxvalue = ((DateTime[])xvalue).Select(x => x.ToOADate()).ToArray();
            //}
            //else
            //{
            //    dxvalue = xvalue;
            //}
            //if (changePts != null)
            //{
            //    cmnd.AppendLine("refe 1 &");
            //    foreach (var item in changePts)
            //    {
            //        Console.WriteLine("Change Points={0}, Confidence level={1}", item.Index, item.ConfidenceLevel);
            //        cmnd.AppendFormat("{0} &\r\n", dxvalue[item.Index + 1].ToString());
            //    }
            //    cmnd.AppendLine(";");
            //}

            //cmnd.AppendFormat("title \"爐區={0}\";\r\n", num);            
            //cmnd.AppendLine(".");
            //proj.ExecuteCommand(cmnd.ToString());



            //List<double[]> d = new List<double[]>();
            //d.Add(ws.Columns.Item("C3").GetData());
            //d.Add(ws.Columns.Item("C4").GetData());
            //d.Add(ws.Columns.Item("C5").GetData());
            //Mtblib.Tools.GScale[] gg = Mtblib.Tools.MtbTools.GetMinitabGScaleInfo(d, proj, ws);

            // Test
            //Mtb.Application mtbApp = new Mtb.Application();
            //mtbApp.UserInterface.Visible = true;
            //mtbApp.UserInterface.DisplayAlerts = false;
            //Mtb.Project proj = mtbApp.ActiveProject;
            // summary data
            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\01.chipMOS\06.Dataset\Top 3 Cust.mtw");
            // stacked data
            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\Test\test20160611.mtw");
            //Mtb.Worksheet ws = proj.ActiveWorksheet;
            // count data
            //proj.Worksheets.Open(@"D:\Dropbox\Workspace\Test\test20160628.mtw");
            //Mtb.Worksheet ws = proj.ActiveWorksheet;

            ////Scatter plot
            //Plot p = new Plot(proj, ws);
            //p.YVariables = "C4 C5";
            //p.XVariables = "C3";
            //p.GraphType = Plot.MultipleGraphType.Overlay;
            //p.YScale.SecScale.Variable = "C5";
            //p.YScale.SecScale.Label.Text= "HEHEHE";
            //p.YScale.Label.Text = "AAAAA";
            //p.XScale.Ticks.SetLabels(new string[] { "\"A\"","\"B\"" });
            //p.Connectline.Visible = true;
            //Console.WriteLine(p.GetCommand());
            //proj.ExecuteCommand(p.GetCommand());



            //HighlevelBarLinePlot hchart = new HighlevelBarLinePlot(proj,ws);
            //hchart.chart.Variables = "C3";
            //hchart.chart.GroupingVariables = "C1 C2";
            //hchart.chart.Bar.GroupingBy = "C2";
            //hchart.chart.FuncType = Chart.ChartFunctionType.MEAN;
            //hchart.chart.Title.Text = "AAA";
            //hchart.chart.XScale.Ticks.TShow = 1;
            //hchart.Run();


            // Chart
            //using (Chart barchart = new Chart(proj, ws))
            //{
            //    // Chart 的基本設定 (Two-way table)                
            //    //barchart.BarsRepresent = Chart.ChartRepresent.TWO_WAY_TABLE;
            //    //barchart.Variables = "C4-C7";
            //    //barchart.GroupingVariables = "C1";

            //    //Chart 的基本設定 (stacked data)
            //    //barchart.FuncType = Chart.ChartFunctionType.MEAN;
            //    //barchart.BarsRepresent = Chart.ChartRepresent.A_FUNCTION_OF_A_VARIABLE;
            //    //barchart.Variables = "C3";
            //    //barchart.GroupingVariables = "C1-C2";

            //    //Chart 的基本設定 (count data)
            //    barchart.BarsRepresent = Chart.ChartRepresent.COUNT_OF_UNIQUE_VALUES;
            //    barchart.Variables = "C1";
            //    barchart.GroupingVariables = "C2";

            //    barchart.AdjDatlabAtStackBar = true;
            //    barchart.DataLabel.Visible = true;
            //    barchart.DataLabel.DatlabType = Datlab.DisplayType.Column;
            //    barchart.StackType = Chart.ChartStackType.Stack;
            //    barchart.XScale.Label.Text = "AAAAA";
            //    barchart.XScale.Ticks.TShow = 1;
            //    barchart.YScale.Label.Text = "BBBB";
            //    barchart.Bar.AssignAttributeByVariables = true;
            //    //barchart.Panel.PaneledBy = "C1";
            //    //barchart.Panel.RowColumn = new int[] { 3, 1 };
            //    barchart.FootnoteLst.Add(new Footnote() { Text = "CCCCC" });
            //    //barchart.FigureRegion.SetCoordinate(0, 1, 0, 0.6);
            //    barchart.Title.Visible = false;
            //    barchart.Title.Text = "xxxx";
            //    barchart.ShowSeparateSubTitle = false;
            //    barchart.ShowPersonalSubTitle = false;
            //    barchart.XScale.Refes.Values = 1.5;
            //    barchart.XScale.Refes.Labels = "\"AAAA\"";
            //    barchart.YScale.Refes.Values = new double[]{100,120};
            //    barchart.YScale.Refes.Color = new int[] { 3, 4 };

            //    string path1 = MtbTools.BuildTemporaryMacro("testchart.mac", barchart.GetCommand());

            //    System.Text.StringBuilder cmnd = new System.Text.StringBuilder();
            //    cmnd.AppendFormat("%\"{0}\" {1};\r\n", path1,
            //        string.Join(" &\r\n", ((Mtb.Column[])barchart.Variables).Select(x => x.SynthesizedName).ToArray())
            //        );
            //    if (barchart.GroupingVariables != null)
            //    {
            //        cmnd.AppendFormat("group {0};\r\n",
            //        string.Join(" &\r\n", ((Mtb.Column[])barchart.GroupingVariables).Select(x => x.SynthesizedName).ToArray())
            //        );
            //    }
            //    if (barchart.Panel.PaneledBy != null)
            //    {
            //        cmnd.AppendFormat("pane {0};\r\n",
            //        string.Join(" &\r\n", (string[])barchart.Panel.PaneledBy)
            //        );
            //    }
            //    if (barchart.DataLabel.LabelColumn != null)
            //    {
            //        cmnd.AppendFormat("datlab {0};\r\n", barchart.DataLabel.LabelColumn);
            //    }
            //    cmnd.Append(".");
            //    string path2 = MtbTools.BuildTemporaryMacro("mycode.mtb", cmnd.ToString());
            //    proj.ExecuteCommand(string.Format("exec \"{0}\" 1", path2));
            //}




            ////Boxplot
            //BoxPlot boxplot = new BoxPlot(proj, ws);
            //boxplot.Mean.Visible = true;
            //boxplot.Mean.Type = 6;
            //boxplot.Mean.Color = MtbColor.DarkSkyBlue;
            //boxplot.CMean.Visible = true;
            //boxplot.Individual.Visible = true;
            //boxplot.IQRBox.Visible = false;
            //boxplot.Whisker.Visible = false;
            //boxplot.Outlier.Visible = false;
            //boxplot.Variables = "C3";
            //boxplot.GroupingVariables = "C1 C2";
            //boxplot.XScale.Label.Visible = false;
            //boxplot.XScale.Ticks.TShow = new int[] { 1, 2 };
            //boxplot.XScale.Ticks.HideAllTick = true;
            //boxplot.FootnoteLst.Add(
            //    new Footnote()
            //    {
            //        Text = "AAAAAA"
            //    });
            //boxplot.MeanDatlab.Visible = true;
            //LabelPosition lpos = new LabelPosition(1, "");
            //boxplot.MeanDatlab.PosititionList.Add(lpos);
            ////boxplot.GraphPath = @"D:\test.jpg";
            //boxplot.GraphRegion.SetCoordinate(10, 4);
            //boxplot.Title.Text = "BoxxxxxPlot";
            //boxplot.FigureRegion.SetCoordinate(0, 1, 0.6, 1);

            //System.Text.StringBuilder cmnd = new System.Text.StringBuilder();
            //cmnd.AppendLine("layout.");
            //cmnd.Append(barchart.GetCommand());
            //cmnd.Append(boxplot.GetCommand());
            //cmnd.AppendLine("endl");
            //proj.ExecuteCommand(cmnd.ToString());

            //proj.ExecuteCommand(boxplot.GetCommand() + ".");
            //Datlab dd = new Datlab();






            //string text = "C1-C5";
            //Regex regex = new Regex(@"('.*?'|C[\d]+)-('.*?'|C[\d]+)");
            //if (regex.IsMatch(text))
            //{
            //    MatchCollection mm = regex.Matches(text);

            //    foreach (Match m in mm)
            //    {
            //        for (int i = 0; i < m.Groups.Count; i++)
            //        {
            //            Console.WriteLine(m.Groups[i].Value);
            //        }
            //    }
            //}



            Application.Run(new Form1());
        }

        public static double Sum(dynamic arr)
        {
            double[] a = arr;
            return a.Sum();
        }
        public static double Mean(dynamic arr)
        {
            double[] a = arr;
            return a.Where(x => x < 1.23456E+30).Average();
        }

        public static string[] method(params object[] args)
        {
            string[] _coord = null;
            if (args.Length != 2 && args.Length != 4) throw new ArgumentException("Annotaion line 有不正確的參數個數");

            _coord = args.Where(x => x != null).Select(x => x.ToString()).
                Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray();

            if (_coord.Length != args.Length)
            {

            }
            return _coord;
        }

        public static Mtb.Column[] GetMatchColumns(string colsText, Mtb.Worksheet ws)
        {
            //
            // 如果輸入的字串是 empty string, null or white space 丟出例外 
            //
            if (string.IsNullOrEmpty(colsText) || string.IsNullOrWhiteSpace(colsText))
            {
                throw new ArgumentNullException("Too few items.", new Exception());
            }

            //
            // 準備 Minitab 欄位
            //
            Mtb.Column[] cols = ws.Columns.Cast<Mtb.Column>().ToArray();
            string[] colLabel = cols.Select(x => x.Label).ToArray();
            string[] colNames = cols.Select(x => x.Name).ToArray();
            string[] colId = cols.Select(x => x.SynthesizedName).ToArray();
            List<Mtb.Column> selCols = new List<Mtb.Column>();


            Regex regex = new Regex(@"(?<=\s|^)(([']).*?([']))(?=\s|$)|(?<=\s|^)(.*?)(?=\s|$)");
            //
            // (?<=\s|^)(([']).*?([']))(?=\s|b): 處理用 quoted name
            // ==> 取得後判斷是否有違法的字元
            // (?<=\s|^)(.*?)(?=\s|$): 處理 non-quoted name
            // ==> 取得後判斷是否是 1)遺漏 quote 2)有違法字元
            // 
            // 違法字元包含: ', # 或單獨一個*
            //
            MatchCollection matches = regex.Matches(colsText);
            Regex invalidRegex = new Regex(@"['#]+");

            foreach (Match match in matches)
            {
                string matchString = match.Groups[0].Value;
                string name;
                //該 pattern 也會抓到 empty string, 所以要另外處理 ==> 跳過
                if (string.IsNullOrWhiteSpace(match.Groups[0].Value) || string.IsNullOrEmpty(match.Groups[0].Value))
                {
                    continue;
                }

                if (match.Groups[1].Success == true) // quoted name
                {
                    if (match.Groups[1].Value.Length == 2) //表示只有 ''
                    {
                        throw new FormatException("No characters in name.");
                    }
                    else if (match.Groups[1].Value.Length > 2)
                    {
                        name = match.Groups[1].Value;
                        name = name.Substring(1, name.Length - 2); // 取得 quote 內的文字
                        if (string.IsNullOrWhiteSpace(name.Substring(0, 1)) ||
                            string.IsNullOrWhiteSpace(name.Substring(name.Length - 1, 1))) //判斷是否有空白在頭尾
                        {
                            throw new FormatException("Leading or trailing blanks in name.");
                        }
                        else if (invalidRegex.IsMatch(name) || name.Equals("*"))
                        {
                            throw new FormatException(
                                string.Format("There is an invalid character in this quoted name: {0}",
                                match.Groups[1].Value));
                        }
                        else
                        {
                            //判斷該名稱是否在現有工作表中的 label 中, 忽略 case
                            if (!colLabel.Contains(name, StringComparer.OrdinalIgnoreCase))
                            {
                                throw new ArgumentException(
                                string.Format("There is no variable by this name: {0}",
                                match.Groups[1].Value));
                            }
                            else // 沒問題就加入選擇欄位清單
                            {
                                selCols.Add(ws.Columns.Item(name));
                            }
                        }
                    }
                }
                else // non-quoted name or 連續表示式
                {
                    name = match.Groups[4].Value;
                    // 先判斷連續表示式
                    // 判斷是否為連續表示式 C1-C3 or 'A'-C3
                    // ('.*?'|C[\d]+)-('.*?'|C[\d]+)
                    Regex _regex = new Regex(@"('.*?'|C[\d]+)-('.*?'|C[\d]+)", RegexOptions.IgnoreCase);
                    if (_regex.IsMatch(name))
                    {
                        MatchCollection _mcollection = _regex.Matches(name);
                        List<Mtb.Column> _cols = new List<Mtb.Column>();
                        foreach (Match m in _mcollection) //應該有兩個 match
                        {
                            _cols.AddRange(GetMatchColumns(m.Groups[1].Value, ws).ToList());
                            _cols.AddRange(GetMatchColumns(m.Groups[2].Value, ws).ToList());
                        }
                        int start = int.Parse(new Regex(@"C([\d]+)").Match(_cols[0].SynthesizedName).Groups[1].Value);
                        int end = int.Parse(new Regex(@"C([\d]+)").Match(_cols[1].SynthesizedName).Groups[1].Value);
                        int step = start < end ? 1 : -1;
                        for (int i = start; start > end ? i >= end : i <= end; i += step)
                        {
                            selCols.Add(ws.Columns.Item(i));
                        }

                    }
                    else // 非連續表示式
                    {
                        if (Regex.IsMatch(name, "^'|'$")) //邊邊有一個 quote 或是裡面有多個 quote 都會被抓進來
                        {
                            throw new FormatException(
                                    string.Format("Missing or stray quote in name: {0}", name));
                        }
                        else if (invalidRegex.IsMatch(name))
                        {
                            throw new FormatException(
                                string.Format("This name contains invalid character(s): {0}", name));
                        }
                        else
                        {
                            if (!colNames.Contains(name, StringComparer.OrdinalIgnoreCase) &&
                                !colId.Contains(name, StringComparer.OrdinalIgnoreCase))
                            {
                                throw new ArgumentException(
                                string.Format("There is no variable by this name: {0}", name));
                            }
                            else // 沒問題就加入選擇欄位清單
                            {
                                selCols.Add(ws.Columns.Item(name));
                            }
                        }
                    }

                }
            }
            return selCols.ToArray();
        }

        public static bool VerifyGraphPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            Regex regex = new Regex(@"^(?:[\w]\:|\\)(\\[^\\\/:\*?<>|]+)+\.(JPEG|JPG|MGF)$",
                RegexOptions.IgnoreCase);
            if (regex.IsMatch(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


