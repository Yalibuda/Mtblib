using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mtblib.Tools
{
    public static class Arithmetic
    {
        /// <summary>
        /// 計算 double 數列中非 missing 的和
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>        
        public static double Sum(IEnumerable<double> array)
        {
            if (!IsAllMissingValue(array) && !IsNullOrZeroLength(array))
            {
                return array.Where(x => x < Tools.MtbTools.MISSINGVALUE).Sum();
            }
            else
            {
                return MtbTools.MISSINGVALUE;
            }

        }
        /// <summary>
        /// 計算 double 數列中非 missing 的算數平均數
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double Mean(this IEnumerable<double> array)
        {
            if (!IsAllMissingValue(array) && !IsNullOrZeroLength(array))
            {
                return array.Where(x => x < Tools.MtbTools.MISSINGVALUE).Average();
            }
            else
            {
                return MtbTools.MISSINGVALUE;
            }

        }
        /// <summary>
        /// 計算 double 數列中非 missing 的最大值
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double Max(IEnumerable<double> array)
        {
            if (!IsAllMissingValue(array) && !IsNullOrZeroLength(array))
            {
                return array.Where(x => x < Tools.MtbTools.MISSINGVALUE).Max();
            }
            else
            {
                return MtbTools.MISSINGVALUE;
            }
        }
        /// <summary>
        /// 計算 double 數列中非 missing 的最小值
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double Min(IEnumerable<double> array)
        {
            if (!IsNullOrZeroLength(array))
            {
                return array.Where(x => x < Tools.MtbTools.MISSINGVALUE).Min();
            }
            else
            {
                return MtbTools.MISSINGVALUE;
            }

        }
        /// <summary>
        /// 計算 double 數列的個數
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double Count(IEnumerable<double> array)
        {
            if (!IsNullOrZeroLength(array))
            {
                return array.Count();
            }
            else
            {
                return Tools.MtbTools.MISSINGVALUE;
            }

        }
        /// <summary>
        /// 計算 double 數列中非 missing 的總數
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double N(IEnumerable<double> array)
        {
            if (!IsNullOrZeroLength(array))
            {
                if (!IsAllMissingValue(array))
                {
                    return array.Where(x => x < Tools.MtbTools.MISSINGVALUE).Count();
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Tools.MtbTools.MISSINGVALUE;
            }

        }
        /// <summary>
        /// 計算 double 數列中 missing 的總數
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double NMiss(IEnumerable<double> array)
        {
            if (!IsNullOrZeroLength(array))
            {
                return array.Count() - N(array);
            }
            else
            {
                return MtbTools.MISSINGVALUE;
            }
        }
        /// <summary>
        /// 計算 double 數列中非 missing 的中位數
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double Median(this IEnumerable<double> array)
        {
            if (IsNullOrZeroLength(array) || IsAllMissingValue(array)) return Tools.MtbTools.MISSINGVALUE;

            double[] excludeMiss = array.Where(x => x < Tools.MtbTools.MISSINGVALUE).OrderBy(x => x).ToArray();
            int count = excludeMiss.Count();
            double median = Tools.MtbTools.MISSINGVALUE;
            if (count % 2 == 0)//Even
            {
                int id = (int)Math.Floor((double)count / 2);
                median = (excludeMiss[id - 1] + excludeMiss[id]) / 2;
            }
            else
            {
                int id = (int)Math.Ceiling((double)count / 2);
                median = excludeMiss[id - 1];
            }
            return median;
        }

        /// <summary>
        /// 計算 double 數列中非 missing 的標準差
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double StdDev(this IEnumerable<double> array)
        {
            if (IsNullOrZeroLength(array) || IsAllMissingValue(array)) return MtbTools.MISSINGVALUE;
            
            double mean = Mean(array);
            double n = N(array);
            if (n <= 1) return MtbTools.MISSINGVALUE;

            var stddev = array.Where(x => x < MtbTools.MISSINGVALUE).Select(x => Math.Pow((x - mean), 2)).Sum() / (n - 1);
            stddev = Math.Sqrt(stddev);
            return stddev;
        }

        /// <summary>
        /// 計算 double 數列中非 missing 的平方和
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double SSQ(this IEnumerable<double> array)
        {
            if (IsNullOrZeroLength(array) || IsAllMissingValue(array)) return MtbTools.MISSINGVALUE;

            var ssq = array.Where(x => x < MtbTools.MISSINGVALUE).Select(x => Math.Pow(x, 2)).Sum();
            return ssq;
        }

        /// <summary>
        /// 計算 double 數列中非 missing 的全距
        /// </summary>
        /// <param name="array">欲處理之數列</param>
        /// <returns></returns>
        public static double Range(this IEnumerable<double> array)
        {
            if (IsNullOrZeroLength(array) || IsAllMissingValue(array)) return Tools.MtbTools.MISSINGVALUE;
            double max = Max(array);
            double min = Min(array);
            return max - min;
        }




        internal static bool IsNullOrZeroLength(IEnumerable<double> array)
        {
            if (array == null || array.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal static bool IsAllMissingValue(IEnumerable<double> array)
        {
            try
            {
                int count = array.Count();
                if (array.Where(x => x >= MtbTools.MISSINGVALUE).Count() == count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
