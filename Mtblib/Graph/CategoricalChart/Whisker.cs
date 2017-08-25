using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;
using Mtblib.Graph.Component;
using Mtblib.Graph.Component.DataView;

namespace Mtblib.Graph.CategoricalChart
{
    public class Whisker : DataView
    {
        public Whisker()
        {
            SetDefault();
        }
        protected override string DefaultCommand()
        {
            StringBuilder cmnd = new StringBuilder();
            if (!this.Visible)
            {
                cmnd.AppendLine("Whisker 0;");
            }
            else
            {
                cmnd.AppendLine("Whisker 1;");
            }

            return cmnd.ToString();
        }

        public override void SetDefault()
        {
            Visible = true;
            GetCommand = DefaultCommand;
        }
        public override object Clone()
        {
            Whisker whisker = new Whisker();
            whisker.Visible = this.Visible;
            return whisker;
        }
    }
}
