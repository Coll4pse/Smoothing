using System;
using System.Linq;

namespace Smoothing
{
    public static class ParabolicApproximation
    {
        public static Polynomial Evaluate(double[] x, double[] y)
        {
            var sqrX = x.Select(x => x * x);
            var cubeX = x.Select(x => x * x * x);
            var quadX = x.Select(x => x * x * x * x);
            var xy = x.Zip(y, (x, y) => x * y);
            var sqrXy = sqrX.Zip(y, (x, y) => x * y);
            var aCoefficients = new Matrix(new double[,]
            {
                {quadX.Sum(), cubeX.Sum(), sqrX.Sum()},
                {cubeX.Sum(), sqrX.Sum(), x.Sum()},
                {sqrX.Sum(), x.Sum(), x.Length}
            });
            var bCoefficients = new Matrix(new double[,]
            {
                {sqrXy.Sum()},
                {xy.Sum()},
                {y.Sum()}
            });
            var abc = Gauss.Evaluate(aCoefficients, bCoefficients);
            return new Polynomial(abc[^1], abc[^2], abc[^3]);
        }
    }
}