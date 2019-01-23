using Mtb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Mtblib.Tools
{
    public enum MtbVarType
    {
        Column,
        Constant,
        Matrix
    }

    public static class MtbTools
    {
        /// <summary>遺失值</summary>
        public const double MISSINGVALUE = 1.23456E+30;
        //public static int[] FILLCOLOR = { 127, 28, 7, 58, 116, 78, 29, 45, 123, 35, 73, 8, 49, 57, 26 };
        public static int[] COLOR = { 64, 8, 9, 12, 18, 34 };
        public static int[] SYMBOLTYPE = { 6, 12, 16, 20, 23, 26, 29 };
        public static int[] LINETPYE = { 1, 2, 3, 4, 5 };

        #region Worksheet 相關

        /// <summary>
        /// 新增 Minitab 變數(Column, Constant, Matrix)
        /// </summary>
        /// <param name="ws">欲新增變數的工作表</param>
        /// <param name="num">欲新增的數量</param>
        /// <param name="mType">變數類型</param>
        /// <returns>string[]:新增的變數ID</returns>
        public static string[] CreateVariableStrArray(Mtb.Worksheet ws, int num, MtbVarType mType)
        {
            int cnt = 0;
            String[] varStr = new String[num]; //num have to large than 1
            try
            {
                switch (mType)
                {
                    case MtbVarType.Column:
                        cnt = ws.Columns.Count;
                        ws.Columns.Add(Quantity: num);
                        for (int i = 0; i < varStr.Length; i++)
                        {
                            varStr[i] = ws.Columns.Item(cnt + 1 + i).SynthesizedName;
                        }
                        break;
                    case MtbVarType.Constant:
                        cnt = ws.Constants.Count;
                        ws.Constants.Add(Quantity: num);
                        for (int i = 0; i < varStr.Length; i++)
                        {
                            varStr[i] = ws.Constants.Item(cnt + 1 + i).SynthesizedName;
                        }
                        break;
                    case MtbVarType.Matrix:
                        cnt = ws.Matrices.Count;
                        ws.Matrices.Add(Quantity: num);
                        for (int i = 0; i < varStr.Length; i++)
                        {
                            varStr[i] = ws.Matrices.Item(cnt + 1 + i).SynthesizedName;
                        }
                        break;
                    default:
                        break;
                }

                return varStr;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + "-" + ex.Message);
                return null;

            }

        }

        /// <summary>
        /// 新增 Minitab 欄位並回傳新增的欄位陣列
        /// </summary>
        /// <param name="ws">欲新增變數的工作表</param>
        /// <param name="num">欲新增的數量</param>
        /// <returns>Mtb.Column[]:新增的欄位ID</returns>
        public static Mtb.Column[] CreateColumnArray(Mtb.Worksheet ws, int num)
        {
            
            int cnt = 0;
            Mtb.Column[] varCols = new Mtb.Column[num]; //num have to large than 1
            try
            {
                cnt = ws.Columns.Count;
                ws.Columns.Add(Quantity: num);
                for (int i = 0; i < varCols.Length; i++)
                {
                    varCols[i] = ws.Columns.Item(cnt + 1 + i);
                }                

                return varCols;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + "-" + ex.Message);
                return null;

            }

        }

        /// <summary>
        /// 取得當下工作表中所有的名稱(Label)
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static string[] GetAllColumnsName(Mtb.Worksheet ws)
        {
            if (ws.Columns.Count == 0)
                return null;
            Mtb.Columns cols = ws.Columns;
            string[] colNames = cols.Cast<Mtb.Column>().Select(x => x.Label).ToArray();
            return colNames;
        }

        /// <summary>
        /// 將文字欄位內容轉換成 Minitab Columns
        /// </summary>
        /// <param name="colsText">來源文字, 包含欄位字串</param>
        /// <param name="ws">用來核對文字的工作表</param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
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
                        List<Mtb.Column> _cols = new List<Column>();
                        foreach (Match m in _mcollection) //應該有兩個 match
                        {
                            _cols.AddRange(GetMatchColumns(m.Groups[1].Value, ws).ToList());
                            _cols.AddRange(GetMatchColumns(m.Groups[2].Value, ws).ToList());
                        }
                        MatchCollection mmmm = new Regex(@"C([\d]+)").Matches(_cols[0].SynthesizedName);
                        int start = int.Parse(new Regex(@"C([\d]+)").Matches(_cols[0].SynthesizedName)[0].Groups[1].Value);
                        int end = int.Parse(new Regex(@"C([\d]+)").Matches(_cols[1].SynthesizedName)[0].Groups[1].Value);
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

        /// <summary>
        /// 將輸入的資料(dynamic)轉換成 Minitab Columns，合法的輸入為一(string/Mtb.Column)或多
        /// 個(string[]/Mtb.Column[])欄位，也可使用連續輸入表示式(string)，如: C1-C3，
        /// 可用單引號名稱或是 Column id。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ws"></param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static Mtb.Column[] GetMatchColumns(dynamic value, Mtb.Worksheet ws)
        {
            List<Mtb.Column> _cols = new List<Column>();
            if (value is string)
            {
                _cols = MtbTools.GetMatchColumns(value, ws);
            }
            else if (value is Mtb.Column)
            {
                _cols.Add(value);
            }
            else if (value is Mtb.Column[])
            {
                _cols = ((Mtb.Column[])value).ToList();
            }
            else if (value is string[])
            {
                List<Mtb.Column> cols = new List<Mtb.Column>();
                foreach (string item in value)
                {
                    cols.AddRange(MtbTools.GetMatchColumns(item, ws).ToList());
                }
                _cols = cols;
            }
            return _cols.ToArray();
        }

        /// <summary>
        /// 指定工作表中特定的欄位名稱轉換成特定的 Local Macro variables。該功能用於建立 local variable，將
        /// DataView 物件中 GroupingBy 的 variables 換成 macro variables，以取得合適的 code。
        /// </summary>
        /// <param name="convertCols">要轉換的欄位名稱</param>
        /// <param name="originalCols">轉換前欄位名稱(col name or col id)</param>
        /// <param name="codedCols">轉換後欄位名稱(macro variable name)</param>
        /// <param name="ws">欄位所在的工作表</param>
        /// <returns></returns>
        public static string[] ConvertToMacroCodedName(string[] convertCols, Mtb.Column[] originalCols, string[] codedCols, Mtb.Worksheet ws)
        {
            //
            // 實際應用的時候，可能會有一對多的狀況，所以暫不考慮使用 Dictionary。
            // E.g. stat y; by x x. 是可行的程式碼
            //

            /*
             * 所有變數不可為 null             
             * 
             */
            if (convertCols == null || originalCols == null || codedCols == null || ws == null)
            {
                throw new ArgumentNullException("輸入的變數不可為 Null");
            }

            /* 
             * 判斷是否存在於指定的工作表中
             * 
             */
            Mtb.Column[] allCols = ws.Columns.Cast<Mtb.Column>().ToArray();
            bool isSubset;
            isSubset = !originalCols.Except(allCols).Any();
            if (!isSubset)
            {
                throw new ArgumentException(
                    string.Format("轉換的來源陣列不存在於指定的工作表({0})中", ws.Name));
            }

            /*
             * 判斷...
             * 1. 轉前後長度是否相同?
             * 2. 是否有1:多的問題 (多:1可)
             * 
             */
            if (originalCols.Length != codedCols.Length)
                throw new ArgumentException("用於轉換 local macro variables 的陣列長度不一樣");

            // 建立 Conversion table
            Dictionary<Mtb.Column, string> converTable = new Dictionary<Column, string>();
            try
            {
                for (int i = 0; i < originalCols.Length; i++)
                {
                    converTable.Add(originalCols[i], codedCols[i]);
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("轉換成 local macro variables 的陣列中含有 Null");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("轉換成 local macro variables 的陣列有1對多的風險");
            }

            // 將要轉換的文字陣列轉換成 Mtb.Column
            // 先判斷是否都在指定欄位中
            Mtb.Column[] recodeCols = GetMatchColumns(convertCols, ws);
            isSubset = !recodeCols.Except(originalCols).Any();
            if (!isSubset)
            {
                throw new ArgumentException(
                    string.Format("要轉換的陣列不存在於轉換陣列中", ws.Name));
            }

            List<string> output = new List<string>();
            foreach (Mtb.Column col in recodeCols)
            {
                output.Add(converTable[col]);
            }

            return output.ToArray();
        }

        /// <summary>
        /// 將指定的 Minitab Columns 轉換成 DataTable，方便資料處理和查詢(考量效率，不適用於大量的資料)
        /// 回傳的欄位名稱使用 Minitab SynthesizedName
        /// </summary>
        /// <param name="cols">Minitab Column 物件</param>
        /// <returns></returns>
        public static DataTable GetDataTableFromMtbCols(Mtb.Column[] cols)
        {
            DataTable dt = new DataTable();
            foreach (var col in cols)
            {
                switch (col.DataType)
                {
                    case Mtb.MtbDataTypes.DataUnassigned:
                        throw new ArgumentNullException(string.Format("{0}欄位中無任何資料", col.Name));
                    case Mtb.MtbDataTypes.DateTime:
                        dt.Columns.Add(col.SynthesizedName, typeof(DateTime));
                        break;
                    case Mtb.MtbDataTypes.Numeric:
                        dt.Columns.Add(col.SynthesizedName, typeof(double));
                        break;
                    case Mtb.MtbDataTypes.Text:
                        dt.Columns.Add(col.SynthesizedName, typeof(string));
                        break;
                    default:
                        break;
                }
            }
            var data = cols.Select(x => x.GetData()).ToArray();
            int rowCnt = cols.Select(x => x.RowCount).Max();
            int colCnt = cols.Length;

            for (int i = 0; i < rowCnt; i++)
            {
                dt.Rows.Add();
            }
            for (int c = 0; c < colCnt; c++)
            {
                var coldata = data[c];
                for (int r = 0; r < coldata.Length; r++)
                {
                    if (r > coldata.Length - 1)
                    {
                        dt.Rows[r][c] = DBNull.Value;
                    }
                    else
                    {
                        dt.Rows[r][c] = coldata[r];
                    }

                }
            }
            return dt;
        }


        /// <summary>
        /// 將指定的 DataTable 匯入至 Minitab Worksheet
        /// </summary>
        /// <param name="dt">欲匯入的 DataTable</param>
        /// <param name="ws">存放資料的 Minitab Worksheet</param>
        public static void InsertDataTableToMtbWs(DataTable dt, Mtb.Worksheet ws)
        {
            Mtblib.Tools.MtbTools.CreateVariableStrArray(ws, dt.Columns.Count, Mtblib.Tools.MtbVarType.Column);
            DataRow[] rows = dt.Rows.Cast<DataRow>().ToArray();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn dcol = dt.Columns[i];
                Mtb.Column mtbcol = ws.Columns.Item(i + 1);
                switch (Type.GetTypeCode(dcol.DataType))
                {
                    case TypeCode.DateTime:
                        mtbcol.SetData(rows.Select(r => r[dcol.ColumnName] == DBNull.Value ?
                            DateTime.MaxValue : (DateTime)r[dcol.ColumnName]).ToArray());
                        break;
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Empty:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                        mtbcol.SetData(rows.Select(r => r[dcol.ColumnName] == DBNull.Value ?
                           MISSINGVALUE : Convert.ToDouble(r[dcol.ColumnName])).ToArray());
                        break;
                    default:
                        mtbcol.SetData(rows.Select(r => r.Field<string>(dcol.ColumnName)).ToArray());
                        break;
                }
                mtbcol.Name = dcol.ColumnName;
            }
        }        

        #endregion

        #region Project 相關
        /// <summary>
        /// 在暫存區建立臨時的暫存巨集
        /// </summary>
        /// <param name="fileName">輸入檔名、副檔名</param>
        /// <param name="cmndStr">巨集指令內容</param>
        /// <returns>string: 檔案路徑</returns>
        public static string BuildTemporaryMacro(string fileName, string cmndStr)
        {
            string fullpath;
            string location =
                System.IO.Path.Combine(Environment.GetEnvironmentVariable("tmp"),
                "Minitab");
            if (!System.IO.Directory.Exists(location))
                System.IO.Directory.CreateDirectory(location);

            fullpath = System.IO.Path.Combine(location, fileName);

            System.IO.FileStream fs = new System.IO.FileStream(fullpath, System.IO.FileMode.Create);
            fs.Close();

            System.IO.StreamWriter sw;
            sw = new System.IO.StreamWriter(fullpath, false, System.Text.Encoding.Unicode);
            sw.Write(cmndStr);
            sw.Close();
            return fullpath;
        }

        #endregion

        #region 陣列轉換

        /// <summary>
        /// 將輸入的變數轉換成整數陣列
        /// </summary>
        /// <param name="d">可為 null,單一(int)或多個(int[])可轉換整數的值</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        internal static int[] ConvertInputToIntArray(dynamic d)
        {
            int[] output = null;
            if (d == null)
            {
                return output;
            }
            else if (d is Array)
            {
                try
                {
                    IEnumerable em = d as IEnumerable;
                    output = em.Cast<object>().Select(x => int.Parse(x.ToString())).ToArray();
                }
                catch
                {
                    throw new ArgumentException("Invalid input when convert to integer array.");
                }
            }
            else
            {
                try
                {
                    output = new int[] { int.Parse(d.ToString()) };
                }
                catch
                {
                    throw new ArgumentException("Invalid value when convert to integer.");
                }
            }
            return output;
        }

        /// <summary>
        /// 將輸入的變數轉換成數值(float)陣列
        /// </summary>
        /// <param name="d">可為 null,單一(float)或多個(float[])可轉換浮點數的值</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        internal static float[] ConvertInputToFloatArray(dynamic d)
        {
            float[] output = null;
            if (d == null)
            {
                return output;
            }
            else if (d is Array)
            {
                try
                {
                    IEnumerable em = d as IEnumerable;
                    output = em.Cast<object>().Select(x => float.Parse(x.ToString())).ToArray();
                }
                catch
                {
                    throw new ArgumentException("Invalid input when convert to integer array.");
                }
            }
            else
            {
                try
                {
                    output = new float[] { float.Parse(d.ToString()) };
                }
                catch
                {
                    throw new ArgumentException("Invalid value when convert to integer.");
                }
            }
            return output;
        }

        /// <summary>
        /// 將輸入的變數轉換成數值(double)陣列
        /// </summary>
        /// <param name="d">可為 null,單一(double)或多個(double[])可轉為數值的值</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        internal static double[] ConvertInputToDoubleArray(dynamic d)
        {
            double[] output = null;
            if (d == null)
            {
                return output;
            }
            else if (d is Array)
            {
                try
                {
                    IEnumerable em = d as IEnumerable;
                    output = em.Cast<object>().Select(x => double.Parse(x.ToString())).ToArray();
                }
                catch
                {
                    throw new ArgumentException("Invalid input when convert to integer array.");
                }
            }
            else
            {
                try
                {
                    output = new double[] { double.Parse(d.ToString()) };
                }
                catch
                {
                    throw new ArgumentException("Invalid value when convert to integer.");
                }
            }
            return output;
        }

        /// <summary>
        /// 將輸入的變數轉換成文字陣列
        /// </summary>
        /// <param name="d">可為 null,單一(string)或多個(string[])值</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        internal static string[] ConvertInputToStringArray(dynamic d)
        {
            string[] output = null;

            if (d == null)
            {
                output = null;
            }
            else if (d is Array)
            {
                object[] o = new object[d.Length];
                d.CopyTo(o, 0);
                output = o.OfType<object>().Select(x => x.ToString()).ToArray();

            }
            else if (d.GetType() == typeof(string))
            {
                output = new string[] { d };
            }
            else
            {
                throw new ArgumentException("Invalid input type when convert to string array");
            }
            return output;
        }

        /// <summary>
        /// 取得陣列特定位置的元素
        /// </summary>
        /// <param name="array">來源陣列</param>
        /// <param name="index">要複製的位置</param>
        /// <returns></returns>
        public static object[] CopyArrayWithIndex(Array array, int[] index)
        {
            var arrayWithIndex = array.OfType<object>().Select((x, i) => new { Value = x, Index = i }).ToArray();
            var output = arrayWithIndex.Where(x => index.Contains(x.Index)).Select(x => x.Value).ToArray();
            return output;
        }


        #endregion

        #region 字串判斷
        /// <summary>
        /// 判斷輸入的圖形檔案路徑是否合法
        /// </summary>
        /// <param name="path">路徑= 位置 + 檔名 + 副檔名</param>
        /// <returns></returns>
        public static bool VerifyGraphPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            Regex regex = new Regex(@"^(?:[\w]\:|\\)(\\[^\\\/:\*?<>|]+)+\.(JPEG|JPG|MGF|PNG|TIF|BMP)$",
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

        /// <summary>
        /// 判斷檔案路徑的資料夾是否存在，如果不存在就新增路徑，若已存在就不動作
        /// </summary>
        /// <param name="path">路徑= 位置 + 檔名 + 副檔名</param>
        public static void CreateDirectory(string path)
        {
            Regex regex = new Regex(@"^(?:[\w]\:|\\)(\\[^\\\/:\*?<>|]+)*\\",
                RegexOptions.IgnoreCase);
            string root = regex.Match(path).Groups[0].Value.ToString();
            if (!System.IO.Directory.Exists(root))
            {
                System.IO.Directory.CreateDirectory(root);
            }
        }
        #endregion

        #region 圖形相關
        /// <summary>
        /// 取得 Minitab 繪製座標軸的資訊
        /// </summary>
        /// <param name="info">要計算坐標軸資訊的資料彙整(最大最小)</param>
        /// <param name="proj">Minitab Project</param>
        /// <param name="ws">數據所在的工作表，需透過工作表存取資訊</param>
        /// <returns></returns>
        public static GScale[] GetMinitabGScaleInfo(DataSummaryInfo[] info, Mtb.Project proj, Mtb.Worksheet ws)
        {
            List<GScale> gscaleCollection = new List<GScale>();
            int init = ws.Constants.Count;  // 記錄一開始的常數量，最後要刪除後來增加的那些
            string[] constStr = CreateVariableStrArray(ws, 6 * info.Length, MtbVarType.Constant);

            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("brief 0");
            cmnd.AppendLine("notitle");
            for (int i = 0; i < info.Length; i++)
            {
                int k = 6 * i;
                double max = info[i].Maximum;
                double min = info[i].Minimum;
                string smin = constStr[0 + k], smax = constStr[1 + k], nticks = constStr[2 + k],
                    tmin = constStr[3 + k], tmax = constStr[4 + k], tinc = constStr[5 + k];
                cmnd.AppendFormat("gscale {0} {1};\r\n", min, max);
                cmnd.AppendFormat(" smin {0};\r\n", smin);
                cmnd.AppendFormat(" smax {0};\r\n", smax);
                cmnd.AppendFormat(" ntick {0};\r\n", nticks);
                cmnd.AppendFormat(" tmin {0};\r\n", tmin);
                cmnd.AppendFormat(" tmax {0};\r\n", tmax);
                cmnd.AppendFormat(" tinc {0}.\r\n", tinc);
            }
            cmnd.AppendLine("title");
            cmnd.AppendLine("brief 2");

            string path = BuildTemporaryMacro("gscale.mtb", cmnd.ToString());
            proj.ExecuteCommand(string.Format("exec \"{0}\" 1", path), ws);

            for (int i = 0; i < info.Length; i++)
            {
                int k = 6 * i;
                string smin = constStr[0 + k], smax = constStr[1 + k], nticks = constStr[2 + k],
                    tmin = constStr[3 + k], tmax = constStr[4 + k], tinc = constStr[5 + k];
                gscaleCollection.Add(new GScale
                {
                    SMin = ws.Constants.Item(smin).GetData(),
                    SMax = ws.Constants.Item(smax).GetData(),
                    NTicks = ws.Constants.Item(nticks).GetData(),
                    TMinimum = ws.Constants.Item(tmin).GetData(),
                    TMaximum = ws.Constants.Item(tmax).GetData(),
                    TIncreament = ws.Constants.Item(tinc).GetData()
                });
            }

            for (int i = ws.Constants.Count; i > init; i--)
            {
                ws.Constants.Remove(i);
            }

            return gscaleCollection.ToArray();
        }

        /// <summary>
        /// 取得 Minitab 繪製座標軸的資訊
        /// </summary>
        /// <param name="values">要計算的數據，合法的輸入為 List<double[]></param>
        /// <param name="proj">Minitab Project</param>
        /// <param name="ws">數據所在的工作表，需透過工作表存取資訊</param>
        /// <returns></returns>
        public static GScale[] GetMinitabGScaleInfo(List<double[]> values, Mtb.Project proj, Mtb.Worksheet ws)
        {

            List<double[]> dataCollection = values;

            List<DataSummaryInfo> dataSumInfo = new List<DataSummaryInfo>();
            for (int i = 0; i < dataCollection.Count; i++)
            {
                double[] data = dataCollection[i];
                DataSummaryInfo dInfo = new DataSummaryInfo();
                dInfo.Minimum = data.Where(x => x < MISSINGVALUE).Min();
                dInfo.Maximum = data.Where(x => x < MISSINGVALUE).Max();
                dataSumInfo.Add(dInfo);
            }

            GScale[] gscaleCollection = GetMinitabGScaleInfo(dataSumInfo.ToArray(), proj, ws);

            #region 取消
            //List<GScale> gscaleCollection = new List<GScale>();
            //int init = ws.Constants.Count;  // 記錄一開始的常數量，最後要刪除後來增加的那些
            //string[] constStr = CreateVariableStrArray(ws, 6 * dataCollection.Count, MtbVarType.Constant);

            //StringBuilder cmnd = new StringBuilder();
            //cmnd.AppendLine("brief 0");
            //cmnd.AppendLine("notitle");
            //for (int i = 0; i < dataCollection.Count; i++)
            //{
            //    int k = 6 * i;
            //    double[] data = dataCollection[i];
            //    double max = data.Where(x => x < MISSINGVALUE).Max();
            //    double min = data.Where(x => x < MISSINGVALUE).Min();
            //    string smin = constStr[0 + k], smax = constStr[1 + k], nticks = constStr[2 + k],
            //        tmin = constStr[3 + k], tmax = constStr[4 + k], tinc = constStr[5 + k];
            //    cmnd.AppendFormat("gscale {0} {1};\r\n", min, max);
            //    cmnd.AppendFormat(" smin {0};\r\n", smin);
            //    cmnd.AppendFormat(" smax {0};\r\n", smax);
            //    cmnd.AppendFormat(" ntick {0};\r\n", nticks);
            //    cmnd.AppendFormat(" tmin {0};\r\n", tmin);
            //    cmnd.AppendFormat(" tmax {0};\r\n", tmax);
            //    cmnd.AppendFormat(" tinc {0}.\r\n", tinc);
            //}
            //cmnd.AppendLine("title");
            //cmnd.AppendLine("brief 2");

            //string path = BuildTemporaryMacro("gscale.mtb", cmnd.ToString());
            //proj.ExecuteCommand(string.Format("exec \"{0}\" 1", path), ws);

            //for (int i = 0; i < dataCollection.Count; i++)
            //{
            //    int k = 6 * i;
            //    string smin = constStr[0 + k], smax = constStr[1 + k], nticks = constStr[2 + k],
            //        tmin = constStr[3 + k], tmax = constStr[4 + k], tinc = constStr[5 + k];
            //    gscaleCollection.Add(new GScale
            //    {
            //        SMin = ws.Constants.Item(smin).GetData(),
            //        SMax = ws.Constants.Item(smax).GetData(),
            //        NTicks = ws.Constants.Item(nticks).GetData(),
            //        TMinimum = ws.Constants.Item(tmin).GetData(),
            //        TMaximum = ws.Constants.Item(tmax).GetData(),
            //        TIncreament = ws.Constants.Item(tinc).GetData()
            //    });
            //}

            //for (int i = ws.Constants.Count; i > init; i--)
            //{
            //    ws.Constants.Remove(i);
            //} 
            #endregion

            return gscaleCollection;

        }

        /// <summary>
        /// 取得 Minitab 繪製座標軸的資訊
        /// </summary>
        /// <param name="values">要計算的數據，合法的輸入為 double[]></param>
        /// <param name="proj">Minitab Project</param>
        /// <param name="ws">數據所在的工作表，需透過工作表存取資訊</param>
        /// <returns></returns>
        public static GScale[] GetMinitabGScaleInfo(double[] values, Mtb.Project proj, Mtb.Worksheet ws)
        {

            List<double[]> dataCollection = new List<double[]>();
            dataCollection.Add(values);

            List<DataSummaryInfo> dataSumInfo = new List<DataSummaryInfo>();
            for (int i = 0; i < dataCollection.Count; i++)
            {
                double[] data = dataCollection[i];
                DataSummaryInfo dInfo = new DataSummaryInfo();
                dInfo.Minimum = data.Where(x => x < MISSINGVALUE).Min();
                dInfo.Maximum = data.Where(x => x < MISSINGVALUE).Max();
                dataSumInfo.Add(dInfo);
            }

            GScale[] gscaleCollection = GetMinitabGScaleInfo(dataSumInfo.ToArray(), proj, ws);

            return gscaleCollection;

        }


        /// <summary>
        /// 取得BarChart的預設座標軸資訊
        /// </summary>
        /// <param name="proj">資料所在的 Minitab Project</param>
        /// <param name="ws">資料所在的 Minitab 工作表</param>
        /// <param name="vars">要繪製 Bar Chart 的變數，如果輸入多筆則視為 summarized data 進行堆疊後處理</param>
        /// <param name="gps">用於分群的變數</param>
        /// <param name="pane">用於畫面分割的變數</param>
        /// <param name="conditionExpress">資料條件敘述</param>
        /// <param name="fun">Bar Chart 的處理函數(不輸入則使用 Sum)</param>
        /// <param name="stackType">Bar chart 是否堆疊</param>
        /// <param name="colIngroupingLevel">輸入 summarized data 時，由 data column 
        /// <param name="scaleMaximum">額外指定的最大值</param>
        /// <param name="scaleMinimum">額外指定的最小值</param>
        /// 名稱堆疊成的欄位的分群水準，合法的輸入為1~4。預設放在最內層(4)。</param>
        /// <returns></returns>
        public static GScale GetDataScaleInBarChart(
            Mtb.Project proj, Mtb.Worksheet ws,
            Mtb.Column[] vars,
            Mtb.Column[] gps = null,
            Mtb.Column[] pane = null,
            string conditionExpress = "",
            Func<IEnumerable<double>, double> fun = null,
            Graph.BarChart.Chart.ChartStackType stackType = Graph.BarChart.Chart.ChartStackType.Cluster,
            int colIngroupingLevel = 4,
            double scaleMaximum = MISSINGVALUE,
            double scaleMinimum = MISSINGVALUE
            )
        {
            List<DataSummaryInfo> dataInfos = new List<DataSummaryInfo>();
            List<Mtb.Column> mtbDataCols = new List<Mtb.Column>();
            List<string> colGroups = new List<string>();
            if (pane != null && pane.Length > 0) mtbDataCols.AddRange(pane);
            if (gps != null && gps.Length > 0) mtbDataCols.AddRange(gps);

            //開始堆疊資料，因為需要欄位名，過程中會加入Minitab欄位
            System.Data.DataTable dt = new System.Data.DataTable();
            string colAggregate;
            if (vars.Length > 1) //summarized data
            {
                #region 堆疊 summarized data 和更新分群資訊
                int currColCnt = ws.Columns.Count;
                string[] colstr
                    = Mtblib.Tools.MtbTools.CreateVariableStrArray(ws, 2, Mtblib.Tools.MtbVarType.Column);
                for (int i = 0; i < vars.Length; i++)
                {
                    List<Mtb.Column> _dataCols = new List<Mtb.Column>();
                    _dataCols.AddRange(mtbDataCols);
                    ws.Columns.Item(colstr[0]).Clear();
                    ws.Columns.Item(colstr[0]).SetData(
                        vars[i].SynthesizedName, 1, vars[i].RowCount);
                    _dataCols.Add(ws.Columns.Item(colstr[0]));
                    ws.Columns.Item(colstr[1]).Clear();
                    ws.Columns.Item(colstr[1]).SetData(vars[i].GetData());
                    _dataCols.Add(ws.Columns.Item(colstr[1]));
                    if (i == 0)
                    {
                        dt = Mtblib.Tools.MtbTools.GetDataTableFromMtbCols(_dataCols.ToArray());
                    }
                    else
                    {
                        dt.Merge(Mtblib.Tools.MtbTools.GetDataTableFromMtbCols(_dataCols.ToArray()), true);
                    }
                }
                //取得 Aggregate 的欄位名稱
                colAggregate = colstr[1];

                //取得分群欄位(依順序)                
                if (gps != null && gps.Length > 0) colGroups.AddRange(gps.Select(x => x.SynthesizedName));
                if (colIngroupingLevel > gps.Length) colGroups.Add(colstr[0]);
                else colGroups.Insert(colIngroupingLevel - 1, colstr[0]);
                if (pane != null && pane.Length > 0) colGroups.InsertRange(0, pane.Select(x => x.SynthesizedName));
                //刪除暫存欄位
                for (int i = ws.Columns.Count; i > currColCnt; i--) ws.Columns.Remove(i);
                #endregion
            }
            else // stacked data
            {
                List<Mtb.Column> _dataCols = new List<Mtb.Column>();
                _dataCols.AddRange(mtbDataCols);
                _dataCols.Add(vars[0]);
                dt = Mtblib.Tools.MtbTools.GetDataTableFromMtbCols(_dataCols.ToArray());
                colAggregate = vars[0].SynthesizedName;
                colGroups.AddRange(gps.Select(x => x.SynthesizedName));
            }

            System.Data.DataTable barChartValueResult
                    = Mtblib.Tools.MtbTools.Apply(colAggregate, fun, colGroups.ToArray(), dt);

            #region 計算不同 Stack 條件下的座標軸最大刻度值

            if (stackType == Mtblib.Graph.BarChart.Chart.ChartStackType.Stack)// Stack Bar Chart
            {
                string[] groupsInXAxis = new string[colGroups.Count - 1];
                groupsInXAxis = barChartValueResult.Columns.Cast<System.Data.DataColumn>()
                    .Where((x, i) => i < colGroups.Count - 1).Select(x => x.ColumnName).ToArray();//根據 Stack 以外的欄位再 aggregate 一次
                System.Data.DataTable aggregateValue
                    = Mtblib.Tools.MtbTools.Apply("Value", Mtblib.Tools.Arithmetic.Sum, groupsInXAxis, barChartValueResult);
                double max = aggregateValue.Rows.Cast<System.Data.DataRow>()
                    .Select(x => Convert.ToDouble(x["Value"])).Where(x => x < Mtblib.Tools.MtbTools.MISSINGVALUE).Max();
                Mtblib.Tools.DataSummaryInfo info = new Mtblib.Tools.DataSummaryInfo();
                info.Maximum = max;
                info.Minimum = 0;
                dataInfos.Add(info);
            }
            else// Cluster
            {
                var values = barChartValueResult.Rows.Cast<System.Data.DataRow>()
                    .Select(x => Convert.ToDouble(x["Value"]));
                double min = 0;
                double max = values.Where(x => x < Mtblib.Tools.MtbTools.MISSINGVALUE).Max();
                Mtblib.Tools.DataSummaryInfo info = new Mtblib.Tools.DataSummaryInfo() { Minimum = min, Maximum = max };
                dataInfos.Add(info);
            }
            #endregion

            Mtblib.Tools.GScale[] scaleInfo
                    = Mtblib.Tools.MtbTools.GetMinitabGScaleInfo(dataInfos.ToArray(), proj, ws);

            return scaleInfo[0];
        }

        /// <summary>
        /// 取得類別分布圖 (Boxplot, Individual value plot, Interval plot) 的預設座標軸資訊
        /// <para>&lt;&lt;目前只考慮顯示 Individual point 的 case&gt;&gt;</para>
        /// </summary>
        /// <param name="proj">資料所在的 Minitab Project</param>
        /// <param name="ws">資料所在的 Minitab 工作表</param>
        /// <param name="vars">要繪製類別分布圖的變數，合法的輸入長度為 1，即 Stack data</param>
        /// <param name="gps">用於分群的變數</param>
        /// <param name="pane">用於畫面分割的變數</param>
        /// <param name="conditionExpress">資料條件敘述</param>
        /// <param name="indVisible">是否顯示單一點 individual</param>
        /// <param name="oVisible">是否顯示異常值 outlier</param>
        /// <param name="rangBoxVisible">是否顯示 Range box</param>
        /// <param name="whiskVisible">是否顯示 Whisker </param>
        /// <param name="ciVisible">是否顯示信賴區間</param>
        /// <param name="iqrVisible">是否顯示 IQR Box</param>
        /// <param name="meanVisible">是否顯示平均值點</param>
        /// <param name="medVisible">是否顯示中位數點</param>
        /// <param name="scaleMaximum">額外指定的最大值</param>
        /// <param name="scaleMinimum">額外指定的最小值</param>
        /// <returns></returns>
        public static GScale GetDataScaleInCateChart(
            Mtb.Project proj, Mtb.Worksheet ws,
            Mtb.Column[] vars,
            Mtb.Column[] gps = null,
            Mtb.Column[] pane = null,
            string conditionExpress = "",
            bool indVisible = true,
            bool oVisible = false,
            bool rangBoxVisible = false,
            bool whiskVisible = false,
            bool ciVisible = false,
            bool iqrVisible = false,
            bool meanVisible = false,
            bool medVisible = false,
            double scaleMaximum = MISSINGVALUE,
            double scaleMinimum = MISSINGVALUE
            )
        {
            List<DataSummaryInfo> dataInfos = new List<DataSummaryInfo>();
            List<Mtb.Column> mtbDataCols = new List<Mtb.Column>();
            List<string> colGroups = new List<string>();
            if (pane != null && pane.Length > 0) mtbDataCols.AddRange(pane);
            if (gps != null && gps.Length > 0) mtbDataCols.AddRange(gps);

            System.Data.DataTable dt = new System.Data.DataTable();
            string colAggregate;

            List<Mtb.Column> _dataCols = new List<Mtb.Column>();
            _dataCols.AddRange(mtbDataCols);
            _dataCols.Add(vars[0]);
            dt = Mtblib.Tools.MtbTools.GetDataTableFromMtbCols(_dataCols.ToArray());
            colAggregate = vars[0].SynthesizedName;

            if (indVisible || oVisible || rangBoxVisible || whiskVisible)
            {
                var valueOfColAggre = dt.AsEnumerable().Select(x => Convert.ToDouble(x[colAggregate]));
                double max = valueOfColAggre.Where(x => x < MtbTools.MISSINGVALUE).Max();
                double min = valueOfColAggre.Where(x => x < MtbTools.MISSINGVALUE).Min();
                DataSummaryInfo info = new DataSummaryInfo() { Minimum = min, Maximum = max };
                dataInfos.Add(info);
            }
            else if (ciVisible || iqrVisible)
            {
                throw new NotImplementedException("DataScale for CI or IQR does not implement yet.");
            }
            else
            {
                throw new NotImplementedException("DataScale for Mean or Median does not implement yet.");
            }

            GScale[] scaleInfo
                   = Mtblib.Tools.MtbTools.GetMinitabGScaleInfo(dataInfos.ToArray(), proj, ws);

            return scaleInfo[0];
        }


        /// <summary>
        /// 計算文字群集的尺寸，同時考慮螢幕放大比例
        /// </summary>
        /// <param name="str">要計算尺寸的文字，可以是一個(string) 或多個(string[], List<string>)</param>
        /// <param name="font">字形</param>
        /// <returns></returns>
        public static SizeF[] GetSizeOfString(dynamic str, Font font)
        {
            Control control = new Control();
            Graphics g = control.CreateGraphics();
            float dpiX = g.DpiX;
            float dpiY = g.DpiY;
            int percent = (dpiX == 96 ? 100 : (dpiX == 120 ? 125 : 150));
            Font f = new Font(font.Name, font.Size * 100 / (float)percent);

            string[] allStrings = null;
            if (str is string)
            {
                allStrings = new string[] { str };
            }
            else if (str is List<string>)
            {
                allStrings = str.ToArray();
            }
            else if (str is string[])
            {
                allStrings = str.Clone();
            }
            else
            {
                throw new ArgumentException("不正確的輸入類型");
            }

            SizeF[] sizeCollection = allStrings.Select(x => g.MeasureString(x, f)).ToArray();

            return sizeCollection;

        }
        

        #endregion

        /// <summary>
        /// 計算 Rank 的準則
        /// </summary>
        public enum RankType
        {
            /// <summary>一般的rank, 例如: 3 2 2 4 1 => Rank: 4 2 2 5 1</summary>
            RANK,
            /// <summary>Minitab 使用 tied rank, 例如: 3 2 2 4 1 => Rank: 4 2.5 2.5 5 1</summary>
            TIED_RANK,
            /// <summary>不考慮相同大小，依排序後的順序給 rank, 用於 Minitab Prob. Plot 例如: 3 2 2 4 1 => Rank: 4 2 3 5 1</summary>
            RANK_IN_ROW
        }

        /// <summary>
        /// 對一組包含群組的數據取各組 Rank
        /// </summary>
        /// <param name="values">要排序的數據</param>
        /// <param name="groups">每個數據的群組資訊</param>
        /// <param name="reverse">Rank 是否反向表示 1--> 最大，預設 false</param>
        /// <param name="rtype">Rank 的類型</param>
        /// <returns></returns>
        public static double[] GroupRank(double[] values, string[] groups, bool reverse = false, RankType rtype = RankType.RANK_IN_ROW)
        {
            if (values.Length != groups.Length)
            {
                throw new ArgumentException("Length of input arrays are not identical.");
            }
            var datas = values.Zip(groups, (x, g) => new { Value = x, Name = g }).Select((x, i) => new { Member = x, RowId = i });
            /*
             * 使用 Select, Order 的邏輯是..先選(包含給新值)再排序..所以 Rank 不需要一開始就給
             * 1. 第一個 Select 是把 index 架構出來...
             * 2. 依據 Lab 分群，用 SelectMany (就是全選)，將分群內容先依數字大小排序
             * 3. 先 Select 出新的架構(含 rank，此時順序就是 rank)，再依據 index 排序
             * 
             */
            var rankInRow = reverse ?
                (

                datas.Select((x, i) => new { Member = x.Member, RowId = x.RowId })
                .GroupBy(x => x.Member.Name).SelectMany(g => g.Select(x => x)
                    .OrderByDescending(x => x.Member.Value).Select((x, i) => new
                    {
                        Member = x.Member,
                        RowId = x.RowId,
                        RankInRow = (x.Member.Value >= MISSINGVALUE ? MISSINGVALUE : (double)i + 1)
                    })).OrderBy(x => x.RowId)

                 ) : (

                 datas.Select((x, i) => new { Member = x.Member, RowId = x.RowId })
                 .GroupBy(x => x.Member.Name).SelectMany(g => g.Select(x => x)
                     .OrderBy(x => x.Member.Value).Select((x, i) => new
                     {
                         Member = x.Member,
                         RowId = x.RowId,
                         RankInRow = (x.Member.Value >= MISSINGVALUE ? MISSINGVALUE : (double)i + 1)
                     })).OrderBy(x => x.RowId)

                  );

            var rank = from data in rankInRow
                       select new
                       {
                           RowId = data.RowId,
                           Member = data.Member,
                           RankInRow = data.RankInRow,
                           Rank = reverse ? (double)(from o in rankInRow
                                                     where o.Member.Name == data.Member.Name & o.Member.Value > data.Member.Value
                                                     select o).Count() + 1 :
                                            (double)(from o in rankInRow
                                                     where o.Member.Name == data.Member.Name & o.Member.Value < data.Member.Value
                                                     select o).Count() + 1
                       };
            var tiedRank = from r in rank
                           select new
                           {
                               Member = r.Member,
                               RowId = r.RowId,
                               RankInRow = r.RankInRow,
                               Rank = r.Rank,
                               TiedRank = (from o in rank
                                           where o.Member.Name == r.Member.Name & o.Member.Value == r.Member.Value
                                           select o.RankInRow).Average()

                           };

            switch (rtype)
            {
                case RankType.RANK:
                    return tiedRank.Select(x => x.Rank).ToArray();
                case RankType.TIED_RANK:
                    return tiedRank.Select(x => x.TiedRank).ToArray();
                case RankType.RANK_IN_ROW:
                default:
                    return tiedRank.Select(x => x.RankInRow).ToArray();
            }

        }

        
        /// <summary>
        /// 對 DataTable的特定一個數值欄位by某些群組做數值Summary
        /// </summary>
        /// <param name="colAggregate">DataTable中要處理的資料欄位名稱，Stack data</param>
        /// <param name="colGroups">DataTable中用於分群的資料欄位名稱</param>
        /// <param name="datatable">包含所需資料的 DataTable</param>
        /// <param name="fun">處理的函數</param>
        /// <returns></returns>
        public static DataTable Apply(string colAggregate,
            Func<IEnumerable<double>, double> fun,
            string[] colGroups,
            DataTable datatable)
        {
            var groupData = datatable.AsEnumerable().GroupBy(
                    r => new NTuple<object>(from col in colGroups select r[col]));
            var applyData = groupData.Select(g =>
                new
                {
                    Group = g.Key.Values,
                    Value = fun(g.Select(r => r[colAggregate].GetType()==typeof(double)?r.Field<double>(colAggregate):Convert.ToDouble(r.Field<decimal>(colAggregate))).ToArray())
                }).ToArray();

            //var result = applyData.ToDictionary(x => x.Group, x => x.Value);
            DataTable dt = new DataTable();
            var keyValues = applyData.Select(x => x.Group).FirstOrDefault();
            for (int i = 0; i < keyValues.Count(); i++)
            {
                Type t = keyValues[i].GetType();
                DataColumn dc = new DataColumn("ByVar" + (i + 1), t);
                dt.Columns.Add(dc);
            }
            dt.Columns.Add(new DataColumn("Value", applyData.Select(x => x.Value).FirstOrDefault().GetType()));

            foreach (var item in applyData)
            {
                object[] o = new object[item.Group.Count() + 1];
                for (int i = 0; i < item.Group.Count(); i++)
                {
                    o[i] = item.Group[i];
                }
                o[o.Length - 1] = item.Value;
                dt.Rows.Add(o);
            }
            return dt;
        }

    }
    /// <summary>
    /// Minitab 座標軸資訊
    /// </summary>
    public struct GScale
    {
        /// <summary>座標軸的最小值</summary>
        public double SMin { set; get; }
        /// <summary>座標軸的最大值</summary>
        public double SMax { set; get; }
        /// <summary>座標軸的刻度數</summary>
        public double NTicks { set; get; }
        /// <summary>座標軸的最小刻度值</summary>
        public double TMinimum { set; get; }
        /// <summary>座標軸的最大刻度值</summary>
        public double TMaximum { set; get; }
        /// <summary>座標軸的刻度間距</summary>
        public double TIncreament { set; get; }
    }
    /// <summary>
    /// 基本敘述統計量
    /// </summary>
    public struct DataSummaryInfo
    {
        /// <summary>最小值</summary>
        public double Minimum { set; get; }
        /// <summary>最大值</summary>
        public double Maximum { get; set; }
        /// <summary>平均值</summary>
        public double Mean { get; set; }
        /// <summary>標準差</summary>
        public double StdDev { get; set; }
    }

}

