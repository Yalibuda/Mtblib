using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mtblib.Tools;

namespace Mtblib.Graph.Component.Region
{
    /// <summary>
    /// Minitab 繪圖區的 Graph region
    /// </summary>
    public class GraphRegion : Region
    {
        public GraphRegion()
        {
            SetDefault();
        }

        /// <summary>
        /// 設定 Graph 的尺寸 ( 2個值，依序為 width, heigh)
        /// </summary>
        /// <param name="args"></param>
        public override void SetCoordinate(params object[] args)
        {
            if (args.Length != 2) throw new ArgumentException("Graph 有不正確的參數個數，必須為 2 個!");
            _coord = MtbTools.ConvertInputToDoubleArray(args);
        }

        /// <summary>
        /// 取得 Data region 位置
        /// </summary>
        /// <returns></returns>
        public override double[] GetCoordinate()
        {
            return _coord;
        }

        protected override string DefaultCommand()
        {
            if (_coord == null && AutoSize == false) return ""; //表示要手動卻沒有輸入座標，直接跳出
            StringBuilder cmnd = new StringBuilder();
            cmnd.AppendLine("Graph &");
            if (AutoSize)
            {
                cmnd.AppendLine(";");
            }
            else
            {
                cmnd.AppendLine(string.Join(" &\r\n", _coord) + ";");
            }    

            cmnd.AppendLine("Graph &");
            if (_coord != null) cmnd.AppendLine(string.Join(" &\r\n", _coord) + " &");
            cmnd.AppendLine(";");
            if (Type != null)
            {
                cmnd.AppendLine(string.Format(" Type {0};", Type[0]));
            }
            if (Color != null)
            {
                cmnd.AppendLine(string.Format(" Color {0};", Color[0]));
            }
            if (EType != null)
            {
                cmnd.AppendLine(string.Format(" EType {0};", Type[0]));
            }
            if (EColor != null)
            {
                cmnd.AppendLine(string.Format(" EColor {0};", Color[0]));
            }
            if (ESize != null)
            {
                cmnd.AppendLine(string.Format(" ESize {0};", ESize[0]));
            }
            return cmnd.ToString();

        }

        public override void SetDefault()
        {
            Type = null;
            Color = null;
            EType = null;
            EColor = null;
            ESize = null;
            GetCommand = DefaultCommand;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }

}
