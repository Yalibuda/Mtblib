using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet;
using LinearAlgebra = MathNet.Numerics.LinearAlgebra;
using Mtblib.Tools;

namespace Mtblib.Stat
{
    /// <summary>
    /// <para>
    /// Change-point analysis is used to determined whether a change has taken place.Using Method: Run(doulbe[]) to get the information of change-point
    /// </para>
    /// <para>
    /// This method was proposed by Dr. Wayne A. Taylor
    /// </para>
    /// </summary>
    public static class ChangePoints
    {

        /// <summary>
        /// 回傳數列中最明顯的 Change Point。
        /// </summary>
        /// <param name="d">The numeric array which use to run change point procedure. Missing value (1.23456E+30) element is not allow in the array.</param>
        /// <param name="conf">The confidence level used to test whether it is a significant change point</param>
        /// <param name="N">Number of boostrap</param>
        /// <returns></returns>
        public static ChangePointInfo ChangePointOnSingleCase(double[] d, double conf = 0.9, int N = 1000)
        {
            if (d.Length < 7) return new ChangePointInfo() { Index = -1 };
            if (d.Any(x => x >= MtbTools.MISSINGVALUE)) throw new ArgumentException("數列中包含遺失值");

            LinearAlgebra.Vector<double> xs = LinearAlgebra.Vector<double>.Build.DenseOfArray(d);
            double xbar = xs.Average();
            xs = xs - xbar; //Xi-Xbar
            LinearAlgebra.Vector<double> si
                = LinearAlgebra.Vector<double>.Build.DenseOfArray(MathTool.PartialSum(xs.ToArray()));

            int changepointindex = si.AbsoluteMaximumIndex();
            double sdiff = si.Maximum() - si.Minimum();

            double[] Sdiff_boostrap = new double[N];
            for (int i = 0; i < N; i++)
            {
                double[] si_boostrap = MathTool.PartialSum(MathTool.RandomSample(xs.ToArray()).Cast<double>().ToArray());
                Sdiff_boostrap[i] = si_boostrap.Max() - si_boostrap.Min();
            }
            double confLv = (double)Sdiff_boostrap.Where(x => x < sdiff).Count() / N;

            if (confLv < conf) return new ChangePointInfo() { Index = -1 }; //Not significant

            return new ChangePointInfo() { Index = changepointindex, Sdiff = sdiff, ConfidenceLevel = confLv };
        }

        /// <summary>
        /// 回傳數列中所有 Change Points 的 index
        /// </summary>
        /// <param name="d">數值數列</param>
        /// <returns></returns>
        public static ChangePointInfo[] MainProcedure(Datas[] d, double conf = 0.9, int N = 1000)
        {
            List<ChangePointInfo> changePtList = new List<ChangePointInfo>();

            ChangePointInfo info = ChangePointOnSingleCase(d.Select(x => x.Data).ToArray(), conf, N);

            if (info.Index == -1 || info.Index == 0 || info.Index == d.Length - 1)
            {
                return null;
            }
            else
            {

                int n1 = (int)info.Index; // Length of segment1
                int n2 = d.Length - n1; // Length of segment2
                changePtList.Add(new ChangePointInfo
                {
                    Sdiff = info.Sdiff,
                    Index = d[info.Index].Index, //Change the local index to original index
                    ConfidenceLevel = info.ConfidenceLevel
                });
                Datas[] d1 = new Datas[n1];
                Datas[] d2 = new Datas[n2];
                Array.Copy(d, d1, n1);
                Array.Copy(d, n1, d2, 0, n2);

                ChangePointInfo[] sub1 = MainProcedure(d1, conf, N);
                if (sub1 != null) changePtList.AddRange(sub1);
                ChangePointInfo[] sub2 = MainProcedure(d2, conf, N);
                if (sub2 != null) changePtList.AddRange(sub2);

            }
            return changePtList.ToArray();
        }

        public static ChangePointInfo[] Run(double[] d, double conf = 0.9, int N = 1000)
        {
            var dWithoutMissingVal = d.Select((x, i) => new Datas { Data = x, Index = i })
                .Where(x => x.Data < 1.23456E+30);
            Dictionary<int, int> conversionTableOfIndex
                = dWithoutMissingVal.Select((x, i) => new { oIndex = x.Index, nIndex = i })
                .ToDictionary(x => x.nIndex, x => x.oIndex);

            ChangePointInfo[] allChangePoint = MainProcedure(
                dWithoutMissingVal.ToArray(), conf, N);

            if (allChangePoint != null)
            {

                allChangePoint = allChangePoint.Select(x =>
                    new ChangePointInfo
                    {
                        Index = conversionTableOfIndex[x.Index],
                        Sdiff = x.Sdiff,
                        ConfidenceLevel = x.ConfidenceLevel
                    }).ToArray();
            }

            return allChangePoint;

        }

        /// <summary>
        /// Information of the result in each change point procedure
        /// </summary>
        public struct ChangePointInfo
        {
            public int Index;
            public double Sdiff;
            public double ConfidenceLevel;
        }

        /// <summary>
        /// Structure of Data and the index
        /// </summary>
        public struct Datas
        {
            public int Index;
            public double Data;
        }

    }
}
