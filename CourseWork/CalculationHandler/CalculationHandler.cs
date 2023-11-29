using System;
using Calculations;
using System.Windows;

namespace CourseWork;

internal class CalculationHandler : ICalculationHandler
{
    private DataHandler dh;

    private DataModel Data
    {
		get => dh.Data;
		set { dh.Data = value; }
	}

    public CalculationHandler(DataHandler dh) {
        this.dh = dh;
    }
	public void Calculate(object sender, RoutedEventArgs e)
	{
		try
		{
			Func<double, double> f = x =>
				LagrangeInterpolation.GetValue(Data.GetPoints("fx"), x) -
				LagrangeInterpolation.GetValue(Data.GetPoints("gx"), x);

			var minX = Fibonacci.FindMinimum(
				f,
				Data.GetMinimalX("fx") - Data.GetMinimalX("gx"),
				Data.GetMaximalX("fx") + Data.GetMaximalX("gx"),
				20,
				1e-3
			);
			var minY = f(minX);

			MessageBox.Show(
				$"x: {minX.ToString("0.000")}\ny: {minY.ToString("0.000")}",
				"Result",
				MessageBoxButton.OK,
				MessageBoxImage.Information
			);
		}
		catch (Exception error)
		{
			MessageBox.Show("Error occurred while calculating the result. Please check if you have entered all the points correctly. " + error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}


