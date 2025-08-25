namespace APSIM.Numerics;

public static class MathExtensions
{
    /// <summary>
    /// Perform a stepwise multiply of the values in value 1 with the values in value2.
    /// Returns an array of the same size as value 1 and value 2
    /// </summary>
    public static double[] Multiply(this IReadOnlyList<double> value1, IReadOnlyList<double> value2)
    {
        double[] result = new double[value1.Count];
        if (value1.Count == value2.Count)
        {
            result = new double[value1.Count];
            for (int iIndex = 0; iIndex < value1.Count; iIndex++)
                result[iIndex] = value1[iIndex] * value2[iIndex];
        }
        return result;
    }

    /// <summary>
    /// Perform a stepwise Divide of the values in value 1 with the values in value2.
    /// Returns an array of the same size as value 1 and value 2
    /// </summary>
    public static double[] Divide(this IReadOnlyList<double> value1, IReadOnlyList<double> value2, double errVal = 0.0)
    {
        double[] result = null;
        if (value1.Count == value2.Count)
        {
            result = new double[value1.Count];
            for (int iIndex = 0; iIndex < value1.Count; iIndex++)
                result[iIndex] = MathUtilities.Divide(value1[iIndex], value2[iIndex], errVal);
        }
        return result;
    }

    /// <summary>Add two arrays of values.</summary>
    /// <param name="values1">The first array of values.</param>
    /// <param name="values2">The second array of values.</param>
    public static double[] Add(this IList<double> values1, IList<double> values2)
    {
        double[] result = new double[values1.Count];
        for (int i = 0; i < values1.Count; i++)
            result[i] += values1[i] + values2[i];
        return result;
    }

    /// <summary>Subtract two arrays of values.</summary>
    /// <param name="values1">The first array of values.</param>
    /// <param name="values2">The second array of values.</param>
    /// <returns>The difference between the two arrays.</returns>
    public static double[] Subtract(this IList<double> values1, IList<double> values2)
    {
        double[] result = new double[values1.Count];
        for (int i = 0; i < values1.Count; i++)
            result[i] += values1[i] - values2[i];
        return result;
    }

    /// <summary>Convert layer thicknesses to cumulative thickness.</summary>
    /// <param name="Thickness">The thickness of each layer.</param>
    public static double[] ToCumulative(this IReadOnlyList<double> Thickness)
    {
        // ------------------------------------------------
        // Return cumulative thickness for each layer - mm
        // ------------------------------------------------
        double[] CumThickness = new double[Thickness.Count];
        if (Thickness.Count > 0)
        {
            CumThickness[0] = Thickness[0];
            for (int Layer = 1; Layer != Thickness.Count; Layer++)
                CumThickness[Layer] = Thickness[Layer] + CumThickness[Layer - 1];
        }
        return CumThickness;
    }

    /// <summary>
    /// Constrain the values in value1 to be greater than the values in value2.
    /// </summary>
    public static IReadOnlyList<double> LowerConstraint(this IReadOnlyList<double> values, IReadOnlyList<double> lower, int startIndex = 0)
    {
        if (values.Count != lower.Count)
            throw new Exception("The two arrays must be the same length.");
        var results = values.ToArray();
        for (int iIndex = startIndex; iIndex < values.Count; iIndex++)
            results[iIndex] = Math.Max(values[iIndex], lower[iIndex]);
        return results;
    }

    /// <summary>
    /// Constrain a collection of values to a collection of upper values.
    /// </summary>
    public static IReadOnlyList<double> UpperConstraint(this IReadOnlyList<double> values, IReadOnlyList<double> upper, int startIndex = 0)
    {
        if (values.Count != upper.Count)
            throw new Exception("The two arrays must be the same length.");
        var results = values.ToArray();
        for (int iIndex = startIndex; iIndex < values.Count; iIndex++)
            results[iIndex] = Math.Min(values[iIndex], upper[iIndex]);
        return results;
    }
}