using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet;
using LinearAlgebra = MathNet.Numerics.LinearAlgebra;
using System.Security.Cryptography;

namespace Mtblib.Stat
{
    public static class MathTool
    {
        /// <summary>
        /// 計算數列各元素的累積和
        /// </summary>
        /// <param name="x">要處理的陣列，合法的輸入是 double[]</param>
        /// <returns></returns>
        public static double[] PartialSum(double[] x)
        {
            LinearAlgebra.Vector<double> vx
                = LinearAlgebra.Double.DenseVector.OfArray(x);
            int row = vx.Count;
            int col = row;
            LinearAlgebra.Matrix<double> lmat
                = LinearAlgebra.Matrix<double>.Build.Dense(row, col, 1);
            lmat = lmat.LowerTriangle();

            LinearAlgebra.Vector<double> result
                = lmat.Multiply(vx);
            return result.ToArray();
        }

        /// <summary>
        /// 隨機排序陣列
        /// </summary>
        /// <param name="x">陣列，合法的輸入為string[] 或 double[]</param>
        /// <returns></returns>
        public static object[] RandomSample(dynamic x)
        {
            Type t = x.GetType();
            if (!t.IsArray) throw new ArgumentException("輸入變數的類型不符");
            RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider();
            object[] objArray = new object[x.Length];
            x.CopyTo(objArray, 0);
            objArray = objArray.OrderBy(o => GetNextInt32(rnd)).ToArray();
            return objArray;
            
        }

        /// <summary>
        /// 指定隨機 index
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        static int GetNextInt32(RNGCryptoServiceProvider rnd)
        {
            byte[] randomInt = new byte[4];
            rnd.GetBytes(randomInt);
            return Convert.ToInt32(randomInt[0]);
        }



    }
}
