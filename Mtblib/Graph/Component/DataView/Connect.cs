using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.DataView
{
    public class Connect: DataView
    {
        public Connect()
        {
            SetDefault();
        }

        public override void SetDefault()
        {
            _type = null;
            _color = null;
            _size = null;
            _groupBy = null;
            Visible = true;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            if (!Visible) return "";

            StringBuilder cmnd = new StringBuilder();
            if (_groupBy != null)
            {
                cmnd.AppendLine("Conn &");
                cmnd.AppendLine(string.Join(" &\r\n", _groupBy) + ";");
            }
            else
            {
                cmnd.AppendLine("Conn;");
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

            return cmnd.ToString();
        }

        public override object Clone()
        {
            Connect obj = new Connect();
            if (Type != null) obj.Type = Type.Clone();
            if (Color != null) obj.Color = Color.Clone();
            if (Size != null) obj.Size = Size.Clone();
            if (GroupingBy != null) obj.GroupingBy = GroupingBy.Clone();
            obj.Visible = this.Visible;
            return obj;
        }

        [Obsolete("Connect line 不支援 EType", true)]
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
        [Obsolete("Connect line 不支援 ESize", true)]
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
        [Obsolete("Connect line 不支援 EColor", true)]
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
        [Obsolete("Connect line 不支援 Base", true)]
        public new dynamic Base { set; get; }
    }
}
