using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtb;
using Mtblib.Tools;

namespace Mtblib.Graph.Component
{
    /// <summary>
    /// Minitab Graph 基本設定
    /// </summary>
    public abstract class MGraph : IDisposable
    {
        protected Mtb.Project _proj;
        protected Mtb.Worksheet _ws;

        public MGraph(Mtb.Project proj, Mtb.Worksheet ws)
        {
            _proj = proj;
            _ws = ws;
            ALineLst = new List<Annotation.Line>();
            AMarkerLst = new List<Annotation.Marker>();
            ARectLst = new List<Annotation.Rectangle>();
            ATextLst = new List<Annotation.Textbox>();
            FootnoteLst = new List<Footnote>();
            Title = new Title();
            ShowDefaultFootnote = false;
            ShowDefaultSubTitle = false;
            ShowPersonalSubTitle = false;
            ShowSeparateSubTitle = false;
            DataRegion = new Region.DataRegion();
            FigureRegion = new Region.FigureRegion();
            GraphRegion = new Region.GraphRegion();
            Legend = new Region.Legend();
            WTitle = null;
            GraphPath = null;
            CommandPath = null;
        }
        /// <summary>
        /// 指定或取得 Session folder 上的標題
        /// </summary>
        public string WTitle { set; get; }
        /// <summary>
        /// 指定或取得圖形儲存路徑(位置+檔名+副檔名)，副檔名可以是 JPG, JPEG, MGF.
        /// </summary>
        public string GraphPath { set; get; }
        /// <summary>
        /// 指定或取得 Minitab script 儲存路徑(位置+檔名)
        /// </summary>
        public string CommandPath { set; get; }

        public List<Annotation.Line> ALineLst { set; get; }
        public List<Annotation.Marker> AMarkerLst { set; get; }
        public List<Annotation.Rectangle> ARectLst { set; get; }
        public List<Annotation.Textbox> ATextLst { set; get; }
        /// <summary>
        /// 圖形上的標題
        /// </summary>
        public Title Title { set; get; }
        /// <summary>
        /// 註解
        /// </summary>
        public List<Footnote> FootnoteLst { set; get; }

        /// <summary>
        /// 指定或取得是否要顯示預設的註腳
        /// </summary>
        public bool ShowDefaultFootnote { set; get; }

        /// <summary>
        /// 指定或取得是否要顯示預設的標題 * 已於 Title 物件中執行
        /// </summary>
        //public bool ShowDefaultTitle { set; get; }

        /// <summary>
        /// 指定或取得是否要顯示預設的子標題
        /// </summary>
        public bool ShowDefaultSubTitle { set; get; }

        /// <summary>
        /// 指定或取得是否要顯示個人註腳
        /// </summary>
        public bool ShowPersonalSubTitle { set; get; }

        /// <summary>
        /// 指定或取得是否要顯示 Panel 分群註腳
        /// </summary>
        public bool ShowSeparateSubTitle { set; get; }

        public Region.DataRegion DataRegion { set; get; }
        public Region.FigureRegion FigureRegion { set; get; }
        public Region.GraphRegion GraphRegion { set; get; }
        public Region.Legend Legend { set; get; }

        /// <summary>
        /// 取得 Chart option 的指令碼 (GSave, WTitle)
        /// </summary>
        /// <returns></returns>
        public virtual string GetOptionCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (GraphPath != null)
            {
                if (MtbTools.VerifyGraphPath(GraphPath))
                {
                    cmnd.AppendFormat(" GSave \"{0}\";\r\n", GraphPath);
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@".*?\.(mgf)$");
                    if (regex.IsMatch(GraphPath))
                    {
                        cmnd.AppendLine("  MGF;");
                    }
                    else
                    {
                        cmnd.AppendLine("  JPEG;");
                    }
                    cmnd.AppendLine("  Replace;");
                }
            }
            if (WTitle != null)
            {
                cmnd.AppendFormat(" Wtitle \"{0}\";\r\n", WTitle);
            }

            return cmnd.ToString();
        }
        /// <summary>
        /// 取得 Region 的指令碼(Graph, Data, Figure)
        /// </summary>
        /// <returns></returns>
        public virtual string GetRegionCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (GraphRegion != null)
            {
                cmnd.Append(GraphRegion.GetCommand());
            }
            if (DataRegion != null)
            {
                cmnd.Append(DataRegion.GetCommand());
            }
            if (FigureRegion != null)
            {
                cmnd.Append(FigureRegion.GetCommand());
            }
            return cmnd.ToString();
        }
        /// <summary>
        /// 取得 Annotation 指令碼 (Line, Mark, Rectangle, TextBox 等)
        /// </summary>
        /// <returns></returns>
        public virtual string GetAnnotationCommand()
        {

            StringBuilder cmnd = new StringBuilder();
            foreach (Footnote footnote in FootnoteLst)
            {
                cmnd.Append(footnote.GetCommand());
            }
            foreach (Component.Annotation.Line line in ALineLst)
            {
                cmnd.Append(line.GetCommand());
            }
            foreach (Component.Annotation.Marker marker in AMarkerLst)
            {
                cmnd.Append(marker.GetCommand());
            }
            foreach (Component.Annotation.Rectangle rect in ARectLst)
            {
                cmnd.Append(rect.GetCommand());
            }
            foreach (Component.Annotation.Textbox tbox in ATextLst)
            {
                cmnd.Append(tbox.GetCommand());
            }

            cmnd.Append(Title.GetCommand());

            if (!ShowDefaultFootnote) cmnd.AppendLine("Nodf;");
            if (!ShowDefaultSubTitle) cmnd.AppendLine("Nods;");
            if (!ShowPersonalSubTitle) cmnd.AppendLine("Nope;");
            if (!ShowSeparateSubTitle) cmnd.AppendLine("Nose;");
            cmnd.AppendLine("Nosf;");
            cmnd.AppendLine("Noxf;");

            return cmnd.ToString();
        }
        /// <summary>
        /// 取得 Data option 指令碼，繪製特定條件的資料
        /// </summary>
        /// <returns></returns>
        public virtual string GetDataOptionCommand()
        {
            throw new NotImplementedException();
        }

        public abstract void SetDefault();
        protected abstract string DefaultCommand();
        public Func<string> GetCommand { set; get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
            GC.Collect();
            _proj = null;
            _ws = null;
            
        }
        ~MGraph()
        {
            Dispose(false);
        }
    }
}
