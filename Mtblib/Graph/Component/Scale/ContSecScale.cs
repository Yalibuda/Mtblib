using Mtblib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Graph.Component.Scale
{
    public class ContSecScale : Scale
    {
        public ContSecScale(ScaleDirection scaleDirection)
            : base(scaleDirection)
        {
            Direction = scaleDirection;
            SetDefault();
        }

        protected override string DefaultCommand()
        {
            if (Variable == null) return "";


            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendFormat("Scale {0};\r\n", (int)Direction);
            cmnd.AppendFormat(" Secs {0};\r\n", string.Join("&\r\n", Variable));

            if (_lDisplay != null)
                cmnd.AppendLine(string.Format("LDisplay {0};", string.Join(" ", _lDisplay)));
            if (_hDisplay != null)
                cmnd.AppendLine(string.Format("HDisplay {0};", string.Join(" ", _hDisplay)));
            if (Min < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Min {0};", Min));
            if (Max < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Max {0};", Max));
            cmnd.Append(Ticks.GetCommand());
            cmnd.Append(Refes.GetCommand());
            cmnd.Append(Label.GetCommand());
            return cmnd.ToString();
        }

        private string[] _variables = null;
        /// <summary>
        /// 設定或取得用於第二座標軸的變數，合法的輸入為單一(string)或多個欄位(string[])資訊。Get 時返回 string[]
        /// </summary>
        public dynamic Variable
        {
            set
            {
                _variables = MtbTools.ConvertInputToStringArray(value);
            }
            get
            {
                return _variables;
            }
        }

        public override void SetDefault()
        {
            Variable = null;
            Min = Mtblib.Tools.MtbTools.MISSINGVALUE;
            Max = Mtblib.Tools.MtbTools.MISSINGVALUE;
            Label = new AxLabel(Direction)
            {
                ScalePrimary = ScalePrimary.Secondary,
                Side = 1
            };
            Ticks = new ContTick();
            Refes = new Refe(Direction) { Secondary = true};
            LDisplay = null;
            HDisplay = null;
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            ContSecScale obj = new ContSecScale(Direction);
            if (this.Variable != null) obj.Variable = this.Variable.Clone();
            if (this.LDisplay != null) obj.LDisplay = (int[])this.LDisplay.Clone();
            if (this.HDisplay != null) obj.HDisplay = (int[])this.HDisplay.Clone();
            obj.Min = this.Min;
            obj.Max = this.Max;            
            obj.Label = (AxLabel)this.Label.Clone();
            obj.Ticks = (ContTick)this.Ticks.Clone();
            obj.Refes = (Refe)this.Refes.Clone();
            return obj;
        }        
    }
}
