using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Scale
{
    public class CateScale : Scale
    {
        public CateScale(ScaleDirection scaleDirection)
            : base(scaleDirection)
        {
            Direction = scaleDirection;
            //_scaleType = ScaleType.Categorical;
            SetDefault();
        }
        public override void SetDefault()
        {
            //Min = Mtblib.Tools.MtbTools.MISSINGVALUE;
            //Max = Mtblib.Tools.MtbTools.MISSINGVALUE;
            Label = new AxLabel(Direction);
            Ticks = new CateTick();
            HDisplay = null;
            LDisplay = null;
            Refes = new Refe(Direction);
            GetCommand = DefaultCommand;
        }

        protected override string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (_lDisplay != null)
                cmnd.AppendLine(string.Format("LDisplay {0};", string.Join(" ", _lDisplay)));
            if (_hDisplay != null)
                cmnd.AppendLine(string.Format("HDisplay {0};", string.Join(" ", _hDisplay)));

            //if (Min < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Min {0};", Min));
            //if (Max < MtbTools.MISSINGVALUE) cmnd.AppendLine(string.Format("Max {0};", Max));

            cmnd.Append(Ticks.GetCommand());
            cmnd.Append(Refes.GetCommand());
            cmnd.Append(Label.GetCommand());
            if (cmnd.Length > 0) //如果有設定再加入
                cmnd.Insert(0, string.Format("Scale {0};\r\n", (int)Direction));

            return cmnd.ToString();
        }



        /// <summary>
        /// Scale 的下限
        /// </summary>
        [Obsolete("類別座標軸不支援 Min", true)]
        public new double Min { set; get; }
        /// <summary>
        /// Scale 的上限
        /// </summary>
        [Obsolete("類別座標軸不支援 Max", true)]
        public new double Max { set; get; }


        public override object Clone()
        {
            CateScale obj = new CateScale(Direction);
            if (this.LDisplay != null) obj.LDisplay = (int[])this.LDisplay.Clone();
            if (this.HDisplay != null) obj.HDisplay = (int[])this.HDisplay.Clone();
            obj.Label = (AxLabel)this.Label.Clone();
            obj.Ticks = (CateTick)this.Ticks.Clone();
            obj.Refes = (Refe)this.Refes.Clone();
            return obj;
        }
    }
}
