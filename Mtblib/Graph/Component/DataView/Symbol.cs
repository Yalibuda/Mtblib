using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.DataView
{
    public class Symbol : DataView
    {
        public Symbol()
        {
            SetDefault();
        }        

        public override void SetDefault()
        {
            Type = null;
            Color = null;
            Size = null;
            GroupingBy = null;
            Visible = true;
            DataViewPositionLst = new List<DataViewPosition>();
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (!Visible) return "";

            StringBuilder cmnd = new StringBuilder();
            if (_groupBy != null)
            {
                cmnd.AppendLine("Symbol &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("Symbol;");
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

        public override object Clone()
        {
            Symbol obj = new Symbol();
            if (Type != null) obj.Type = Type.Clone();
            if (Color != null) obj.Color = Color.Clone();
            if (Size != null) obj.Size = Size.Clone();
            if (GroupingBy != null) obj.GroupingBy = GroupingBy.Clone();
            obj.Visible = this.Visible;
            foreach (DataViewPosition pos in DataViewPositionLst)
            {
                obj.DataViewPositionLst.Add((DataViewPosition)pos.Clone());
            }
            return obj;
        }

        [Obsolete("Symbol 不支援 EType",true)]
        public new dynamic EType
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
        [Obsolete("Symbol 不支援 ESize", true)]
        public new dynamic ESize
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
        [Obsolete("Symbol 不支援 EColor", true)]
        public new dynamic EColor
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
        [Obsolete("Symbol 不支援 Base", true)]
        public new dynamic Base { set; get; }
        
    }
}
