using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    public class CateTick : Tick
    {
        public CateTick()
        {
            SetDefault();
        }

        public override void SetTicks(dynamic ticks)
        {
            throw new NotImplementedException("Tick command is not permitted in Categorical tick");
        }

        public override void SetLabels(dynamic labels)
        {
            _labels = MtbTools.ConvertInputToStringArray(labels);
        }

        public override void SetDefault()
        {
            Start = -1;
            Increament = -1;
            FontColor = -1;
            FontSize = -1;
            Bold = false;
            Italic = false;
            Angle = MtbTools.MISSINGVALUE;
            _ticks = null;
            TShow = null;
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();

            if (Start > -1) cmnd.AppendLine(string.Format(" Tstart {0};", NMajor));
            //if (Increament > -1) cmnd.AppendLine(string.Format(" Tincr K {0}", NMinor));
            if (Increament > -1) cmnd.AppendLine(string.Format(" Tincr {0};", Increament));
            if (_labels != null)
            {
                cmnd.AppendLine(" Label &");
                cmnd.AppendLine(string.Join(" &\r\n", _labels) + ";");
            }

            if (FontColor > -1) cmnd.AppendLine(string.Format(" Tcolor {0};", FontColor));
            if (FontSize > 0) cmnd.AppendLine(string.Format(" Psize {0};", FontSize));
            if (Bold) cmnd.AppendLine(" Bold;");
            if (Italic) cmnd.AppendLine(" Italic;");
            if (Underline) cmnd.AppendLine(" Underline;");
            if (Angle < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format(" Angle {0};", Angle));

            if (HideAllTick)
            {
                cmnd.AppendFormat(" TShow;\r\n");
            }
            else if (TShow != null)
            {
                cmnd.AppendFormat(" TShow {0};\r\n", string.Join(" ", TShow));
            }

            return cmnd.ToString();
        }

        public override object Clone()
        {
            CateTick tick = new CateTick();
            //tick.NMajor = this.NMajor;
            //tick.NMinor = this.NMinor;
            tick.Start = this.Start;
            tick.Increament = this.Increament;
            tick.FontSize = this.FontSize;
            tick.FontColor = this.FontColor;
            tick.Bold = this.Bold;
            tick.Italic = this.Italic;
            tick.Underline = this.Underline;
            tick.Angle = this.Angle;
            tick.SetLabels(this.GetLabels());
            return tick;
        }

        public override double Increament
        {
            get
            {
                return base.Increament;
            }
            set
            {
                int parseInt;
                if (!int.TryParse(value.ToString(), out parseInt))
                {
                    throw new ArgumentException("在類別型刻度中，Increament 必須為整數");
                }
                base.Increament = value;
            }
        }
    }
}
