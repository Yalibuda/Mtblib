using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.DataView
{
    public class Bar : DataView
    {
        public Bar()
        {
            SetDefault();
        }

        /// <summary>
        /// 如果使用 Multi-variable 繪圖(2-way table的條件下)，如果要依據各變數給定 Bar 顏色、類型等，
        /// 需要 VASSIGN (Variables assign) 指令；此指令要搭配 Overlay 指令才有用
        /// </summary>
        public bool AssignAttributeByVariables { set; get; }
        protected override string DefaultCommand()
        {
            if (!Visible) return "";

            StringBuilder cmnd = new StringBuilder();
            if (GroupingBy != null)
            {
                cmnd.AppendLine("Bar &");
                cmnd.AppendLine(string.Join(" &\r\n", GroupingBy) + ";");
            }
            else
            {
                cmnd.AppendLine("Bar;");
            }
            if (AssignAttributeByVariables) cmnd.AppendLine("VAss;");
            if (Type != null)
            {
                cmnd.AppendLine("Type &");
                cmnd.AppendLine(string.Join(" &\r\n", Type) + ";");
            }
            if (Color != null)
            {
                cmnd.AppendLine("Color &");
                cmnd.AppendLine(string.Join(" &\r\n", Color) + ";");
            }
            if (EType != null)
            {
                cmnd.AppendLine("EType &");
                cmnd.AppendLine(string.Join(" &\r\n", EType) + ";");
            }
            if (EColor != null)
            {
                cmnd.AppendLine("EColor &");
                cmnd.AppendLine(string.Join(" &\r\n", EColor) + ";");
            }
            if (ESize != null)
            {
                cmnd.AppendLine("ESize &");
                cmnd.AppendLine(string.Join(" &\r\n", ESize) + ";");
            }
            if (Base != null)
            {
                cmnd.AppendLine("Base &");
                cmnd.AppendLine(string.Join(" &\r\n", Base) + ";");
            }

            foreach (DataViewPosition pos in DataViewPositionLst)
            {
                cmnd.AppendLine(pos.GetCommand());
            }

            return cmnd.ToString();
        }

        public override void SetDefault()
        {
            AssignAttributeByVariables = false;
            Type = null;
            Color = null;
            EType = null;
            EColor = null;
            ESize = null;
            Visible = true;
            Base = null;
            GroupingBy = null;
            DataViewPositionLst = new List<DataViewPosition>();
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            Bar bar = new Bar();
            if (Type != null) bar.Type = Type.Clone();
            if (Color != null) bar.Color = Color.Clone();
            if (EType != null) bar.EType = EType.Clone();
            if (EColor != null) bar.EColor = EColor.Clone();
            if (ESize != null) bar.ESize = ESize.Clone();
            if (GroupingBy != null) bar.GroupingBy = GroupingBy.Clone();
            if (Base != null) bar.Base = Base.Clone();
            bar.Visible = this.Visible;
            bar.AssignAttributeByVariables = this.AssignAttributeByVariables;
            foreach (DataViewPosition pos in DataViewPositionLst)
            {
                bar.DataViewPositionLst.Add((DataViewPosition)pos.Clone());
            }
            //bar.GetCommand = this.GetCommand;
            return bar;
        }

        [Obsolete("Bar 不支援 Size 屬性", true)]
        public new dynamic Size
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
