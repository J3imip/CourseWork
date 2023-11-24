namespace Calculations;

public class Point
{
	public double X { get; set; }
	public double Y { get; set; }

	override public string ToString()
	{
		return $"Point: ({X}, {Y})";
	}

	public Point() { }
	public Point(double x, double y)
	{
		X = x;
		Y = y;
	}
}

static public class LagrangeInterpolation
{
	public static double GetValue(List<Point> points, double xi)
	{
		double result = 0; // Initialize result

		for (int i = 0; i < points.Count; i++)
		{
			// Compute individual terms
			// of above formula
			double term = points[i].Y;
			for (int j = 0; j < points.Count; j++)
			{
				if (j != i)
					term = term * (xi - points[j].X) / (points[i].X - points[j].X);
			}

			// Add current term to result
			result += term;
		}
		return result;
	}
}

