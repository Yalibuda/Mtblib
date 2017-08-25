using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Region
{
    public class LegendSection
    {
        int _secId = 1;
        public LegendSection(int k)
        {
            if (k < 1) throw new ArgumentException("Section id 必須是正整數");
            _secId = k;
            SetDefault();
        }
        /// <summary>
        /// 指定或取得 Section 的 Title，只有在多個 Section 時有作用
        /// </summary>
        public string STitle { set; get; }
        /// <summary>
        /// 指定或取得是否要隱藏 Section title
        /// </summary>
        public bool HideSTitle { set; get; }
        /// <summary>
        /// 指定或取得是否要隱藏欄位名稱
        /// </summary>
        public bool HideColumnHeader { set; get; }

        private int[] _rhide = null;
        /// <summary>
        /// 指定或取得 Legend box 中要隱藏的 row，合法的輸入為單一(int)或多個(int[])列數
        /// </summary>
        public dynamic RowHide
        {
            set
            {
                _rhide = MtbTools.ConvertInputToIntArray(value);
            }
            get
            {
                return _rhide;
            }
        }

        /// <summary>
        /// 指定或取得 Legend box 中要隱藏的 col，合法的輸入為單一(int)或多個(int[])行數
        /// </summary>
        public dynamic ColumnHide { set; get; }

        public void SetDefault()
        {
            STitle = null;
            HideSTitle = false;
            HideColumnHeader = false;
            RowHide = null;
            ColumnHide = null;
            GetCommand = DefaultCommand;
        }
        public virtual string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendFormat("Section {0};\r\n",_secId);
            if (STitle != null) cmnd.AppendFormat(" Stitle \"{0}\";\r\n",STitle);
            if (HideSTitle) cmnd.AppendLine(" STHide;");
            if (HideColumnHeader) cmnd.AppendLine(" CHHide;");
            if (RowHide != null) cmnd.AppendFormat(" RHide {0};\r\n",string.Join(" &\r\n", RowHide));
            if (ColumnHide != null) cmnd.AppendFormat(" CHide {0};\r\n", string.Join(" &\r\n", ColumnHide));
            return cmnd.ToString();
        }

        public Func<string> GetCommand { set; get; }

        /// <summary>
        /// 複製 Section 物件，不過 GetCommand 不會被複製
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            LegendSection obj = new LegendSection(_secId);
            obj.STitle = this.STitle;
            obj.HideSTitle = this.HideSTitle;
            obj.HideColumnHeader = this.HideColumnHeader;
            obj.RowHide = this.RowHide;
            obj.ColumnHide = this.ColumnHide;
            return obj;

        }

        /// <summary>
        /// 指定 Legend box 中欄位名稱
        /// </summary>
        /// <param name="colId">Column id in legend box</param>
        /// <param name="text">指定的文字</param>
        public void SetColHeader(int colId, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定 Legend box 內容的文字
        /// </summary>
        /// <param name="rowId">Row id in legend box</param>
        /// <param name="colId">Column id in legend box</param>
        /// <param name="text">指定的文字</param>
        public void SetBodyText(int rowId, int colId, string text)
        {
            throw new NotImplementedException();
        }
    }
}
