﻿namespace Calculations;
static public class Fibonacci
{
	static public double CalculateDerivative(Func<double, double> function, double x, double h = 1e-6)
	{
		double derivative = (function(x + h) - function(x - h)) / (2 * h);
		return derivative;
	}
	static private List<double> CalculateFibonacciTo(int n)
	{
		Dictionary<int, double> memo = new Dictionary<int, double>();

		memo[0] = 1;
		memo[1] = 2;

		for (int i = 2; i <= n; i++)
		{
			double nextFib = memo[i - 1] + memo[i - 2];
			memo[i] = nextFib;
		}

		return memo.Values.ToList();
	}
	static public double FindMinimum(Func<double, double> function, double a, double b, int iter, double eps = 0.0001)
	{
		var fib = CalculateFibonacciTo(iter + 1);
		double d = fib[iter - 1] / fib[iter] * (b - a) + (double)Math.Pow(-1, iter) / fib[iter] * eps;

		double x1 = b - d;
		double x2 = a + d;

		for (int i = 0; i < iter; i++)
		{
			if (function(x1) < function(x2))
			{
				b = x2;
				x2 = b - Math.Abs(x1 - a);
			}
			else
			{
				a = x1;
				x1 = a + Math.Abs(b - x2);
			}

			if (x1 > x2)
			{
				(x1, x2) = (x2, x1);
			}
		}

		return (a + b) / 2;
	}
}