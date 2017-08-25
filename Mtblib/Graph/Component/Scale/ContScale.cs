using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    public class ContScale : Scale
    {
        public ContScale(ScaleDirection scaleDirection)
            : base(scaleDirection)
        {
            Direction = scaleDirection;
            //_scaleType = ScaleType.Continuous;
            //_scalePrimary = scalePrimary;
            SetDefault();
        }

        /// <summary>
        /// 指定或取得第二座標軸的物件
        /// </summary>
        public ContSecScale SecScale { set; get; }

        public override void SetDefault()
        {
            Min = Mtblib.Tools.MtbTools.MISSINGVALUE;
            Max = Mtblib.Tools.MtbTools.MISSINGVALUE;
            Label = new AxLabel(Direction);
            Ticks = new ContTick();
            Refes = new Refe(Direction);
            LDisplay = null;
            HDisplay = null;
            SecScale = new ContSecScale(Direction);
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (LDisplay != null)
                cmnd.AppendLine(string.Format("LDisplay {0};", string.Join(" ", LDisplay)));
            if (HDisplay != null)
                cmnd.AppendLine(string.Format("HDisplay {0};", string.Join(" ", HDisplay)));
            if (Min < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Min {0};", Min));
            if (Max < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Max {0};", Max));

            cmnd.Append(Ticks.GetCommand());
            cmnd.Append(Refes.GetCommand());
            cmnd.Append(Label.GetCommand());
            if (cmnd.Length > 0) //如果有設定再加入
                cmnd.Insert(0, string.Format("Scale {0};\r\n", (int)Direction));
            cmnd.Append(SecScale.GetCommand());
            return cmnd.ToString();
        }

        public override object Clone()
        {
            ContScale obj = new ContScale(Direction);
            if (this.LDisplay != null) obj.LDisplay = (int[])this.LDisplay.Clone();
            if (this.HDisplay != null) obj.HDisplay = (int[])this.HDisplay.Clone();
            obj.Min = this.Min;
            obj.Max = this.Max;
            obj.Label = (AxLabel)this.Label.Clone();
            obj.Ticks = (ContTick)this.Ticks.Clone();
            obj.Refes = (Refe)this.Refes.Clone();
            obj.SecScale = (ContSecScale)this.SecScale.Clone();
            return obj;
        }
    }

}
