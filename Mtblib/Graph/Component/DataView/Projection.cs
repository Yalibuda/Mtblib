using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.DataView
{
    /// <summary>
    /// 圖形中的投影線
    /// </summary>
    public class Projection : DataView
    {
        public Projection()
        {
            SetDefault();
        }
        protected override string DefaultCommand()
        {
            if (!Visible) return "";

            StringBuilder cmnd = new StringBuilder();
            if (_groupBy != null)
            {
                cmnd.AppendLine("Proj &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("Proj;");
            }

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
            if (Size != null)
            {
                cmnd.AppendLine("Size &");
                cmnd.AppendLine(string.Join(" &\r\n", Size) + ";");
            }

            foreach (DataViewPosition pos in DataViewPositionLst)
            {
                cmnd.AppendLine(pos.GetCommand());
            }

            return cmnd.ToString();
        }

        public override void SetDefault()
        {
            Type = null;
            Color = null;
            Size = null;
            Visible = true;
            Base = null;
            GroupingBy = null;
            DataViewPositionLst = new List<DataViewPosition>();
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            Projection obj = new Projection();            
            if (Type != null) obj.Type = Type.Clone();
            if (Color != null) obj.Color = Color.Clone();
            if (Size != null) obj.Size = Size.Clone();
            if (GroupingBy != null) obj.GroupingBy = GroupingBy.Clone();
            if (Base != null) obj.Base = Base.Clone();            
            obj.Visible = this.Visible;
            foreach (DataViewPosition pos in DataViewPositionLst)
            {
                obj.DataViewPositionLst.Add((DataViewPosition)pos.Clone());
            }
            return obj;
        }

        [Obsolete("Projectline 不支援 EType 指令",true)]
        public new dynamic EType
        {
            set { throw new NotImplementedException(); }
            get { throw new NotImplementedException(); }
        }
        [Obsolete("Projectline 不支援 EColor 指令", true)]
        public new dynamic EColor
        {
            set { throw new NotImplementedException(); }
            get { throw new NotImplementedException(); }
        }
        [Obsolete("Projectline 不支援 ESize 指令", true)]
        public new dynamic ESize
        {
            set { throw new NotImplementedException(); }
            get { throw new NotImplementedException(); }
        }
    }
}
