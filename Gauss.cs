using System;

namespace Smoothing
{
    public static class Gauss
    {
        public static double[] Evaluate(Matrix aCoefficients, Matrix bCoefficients)
        {
            (aCoefficients, bCoefficients) = DirectMove(aCoefficients, bCoefficients);
            return ReverseMove(aCoefficients, bCoefficients);
        }

        private static (Matrix, Matrix) DirectMove(Matrix aCoefficients, Matrix bCoefficients)
        {
            for (var i = 0; i < aCoefficients.RowCount - 1; i++)
            {
                var count = 0;
                while (aCoefficients[i, i] == 0 && count < aCoefficients.RowCount - i)
                {
                    aCoefficients = aCoefficients.RowToEnd(i);
                    bCoefficients = bCoefficients.RowToEnd(i);
                    count++;
                }

                if (aCoefficients[i, i] == 0)
                    continue;
                for (var j = i + 1; j < aCoefficients.RowCount; j++)
                {
                    var multiplier = -aCoefficients[j, i] / aCoefficients[i, i];
                    aCoefficients = aCoefficients.SumRows(i, j, multiplier);
                    bCoefficients = bCoefficients.SumRows(i, j, multiplier);
                }
            }

            return (aCoefficients, bCoefficients);
        }

        private static double[] ReverseMove(Matrix aCoefficients, Matrix bCoefficients)
        {
            var solves = new double[bCoefficients.RowCount];
            for (var i = aCoefficients.RowCount - 1; i >= 0; i--)
            {
                solves[i] = bCoefficients[i, 0] / aCoefficients[i, i];

                for (var k = aCoefficients.RowCount - 1; k > i; k--)
                    solves[i] -= aCoefficients[i, k] * solves[k] / aCoefficients[i, i];

                solves[i] = Math.Round(solves[i], 2);
            }

            return solves;
        }
    }
}