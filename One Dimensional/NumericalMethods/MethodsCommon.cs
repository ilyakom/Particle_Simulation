namespace Diploma.NumericalMethods
{
    public static class MethodsCommon
    {
		public static float Solution(float x, float t)
        {
            //if (Math.Abs(x) <= Localize / 2)
            //{
            //    return (4 * Math.Pow(Math.Cos(Math.PI * x / Localize), 2)) / (3 * (Colapse - t));
            //}
            //else
            //    return 0;

            return x > 20 && x <= 25
                ? x - 20
				: x > 10 && x <= 15
				? x - 10
				: x > 5 && x <= 10
				? 15
                : x > 25 && x <= 30
                ? 30 - x 
			    : x > 15 && x <= 20 
	            ? 20 - x : 0;
        }

	    /// <summary>
	    /// Нагреватель
	    /// </summary>
	    /// <param name="param"> Координата </param>
	    public static double HeatGenerator(double param)
	    {
		    return 0;
	    }
	}
}
