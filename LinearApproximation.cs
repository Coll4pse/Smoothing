using System;
using System.Linq;

namespace Smoothing
{
    public static class LinearApproximation
    {
        public static Polynomial Evaluate(double[] x, double[] y)
        {
            var sqrX = x.Select(x => x * x);
            var xy = x.Zip(y, (x, y) => x * y);
            var aCoefficients = new Matrix(new double[,]
            {
                {sqrX.Sum(), x.Sum()},
                {x.Sum(), x.Length}
            });
            var bCoefficients = new Matrix(new double[,]
            {
                {xy.Sum()},
                {y.Sum()}
            });
            var abc = Gauss.Evaluate(aCoefficients, bCoefficients);
            return new Polynomial(abc[^1], abc[^2]);
        }
    }
}