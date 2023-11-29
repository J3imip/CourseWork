using Calculations;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using Point = Calculations.Point;

namespace CourseWork;

internal class GraphPlotter : IGraphPlotter
{
	private double Zoom;
	private double Accuracy;
	private int FibonacciIterations;
	private DataModel dm;
	private Point? MinPoint;
	private PlotModel plotModel;

	public GraphPlotter(
		DataModel dm,
		double Zoom,
		double Accuracy,
		int FibonacciIterations
	)
	{
		this.Zoom = Zoom;
		this.Accuracy = Accuracy;
		this.FibonacciIterations = FibonacciIterations;

		plotModel = new PlotModel();
		
		this.dm = dm;
	}

	public (PlotModel, Point) CreatePlotModel()
	{
		// Adding a horizontal axis (X-axis) at the middle of the screen
		plotModel.Axes.Add(new LinearAxis
		{
			Position = AxisPosition.Bottom,
			Minimum = -Zoom,
			Maximum = Zoom,
			MajorStep = 2,
			MinorStep = 1,
			TickStyle = TickStyle.Outside,
			Title = "X Axis",
			AxislineColor = OxyColors.Black,
			AxislineStyle = LineStyle.Solid, // Show a solid line for the X-axis
			ExtraGridlines = new[] { 0.0 }, // Show a grid line at zero
			ExtraGridlineStyle = LineStyle.Solid, // Style for the grid line at zero
			ExtraGridlineColor = OxyColors.Black, // Color of the grid line at zero
			ExtraGridlineThickness = 1.5,
		});

		// Adding a vertical axis (Y-axis) at the middle of the screen
		plotModel.Axes.Add(new LinearAxis
		{
			Position = AxisPosition.Left,
			Minimum = -Zoom,
			Maximum = Zoom,
			MajorStep = 2,
			MinorStep = 1,
			TickStyle = TickStyle.Outside,
			Title = "Y Axis",
			AxislineColor = OxyColors.Black,
			AxislineStyle = LineStyle.Solid, // Show a solid line for the Y-axis
			ExtraGridlines = new[] { 0.0 }, // Show a grid line at zero
			ExtraGridlineStyle = LineStyle.Solid, // Style for the grid line at zero
			ExtraGridlineColor = OxyColors.Black, // Color of the grid line at zero
			ExtraGridlineThickness = 1.5,
		});

		var minXLimit = dm.GetMinimalX("fx") - dm.GetMinimalX("gx");
		var maxXLimit = dm.GetMaximalX("fx") + dm.GetMaximalX("gx");

		Func<double, double> f = x =>
			LagrangeInterpolation.GetValue(dm.GetPoints("fx"), x) -
			LagrangeInterpolation.GetValue(dm.GetPoints("gx"), x);

		plotModel.Series.Add(
			new FunctionSeries(
				f,
				minXLimit - Zoom,
				maxXLimit + Zoom,
				Accuracy
			)
		);

		var minX = Fibonacci.FindMinimum(
			f,
			minXLimit,
			maxXLimit,
			FibonacciIterations,
			Accuracy
		);

		MinPoint = new Point(minX, f(minX));

		plotModel.Annotations.Add(
			new PointAnnotation { X = MinPoint.X, Y = MinPoint.Y, Text = $"({MinPoint.X:0.000};{MinPoint.Y:0.000})" }
		);

		//focus at the minimum point
		plotModel.Axes[0].Minimum = MinPoint.X - Zoom;
		plotModel.Axes[0].Maximum = MinPoint.X + Zoom;
		plotModel.Axes[1].Minimum = MinPoint.Y - Zoom;
		plotModel.Axes[1].Maximum = MinPoint.Y + Zoom;

		return (plotModel, MinPoint);
	}

	public (PlotModel, Point) UpdateMinPoint(double xMin, double xMax)
	{
		Func<double, double> f = x =>
			LagrangeInterpolation.GetValue(dm.GetPoints("fx"), x) -
			LagrangeInterpolation.GetValue(dm.GetPoints("gx"), x); 

		var fibMinX = Fibonacci.FindMinimum(
			f,
			xMin,
			xMax,
			20
		);

		MinPoint = new Point(fibMinX, f(fibMinX));

		plotModel.Annotations.Clear();
		plotModel.Annotations.Add(
			new PointAnnotation { X = MinPoint.X, Y = MinPoint.Y, Text = $"({MinPoint.X:0.000};{MinPoint.Y:0.000})" }
		);

		plotModel.Axes[0].Minimum = MinPoint.X - Zoom;
		plotModel.Axes[0].Maximum = MinPoint.X + Zoom;
		plotModel.Axes[1].Minimum = MinPoint.Y - Zoom;
		plotModel.Axes[1].Maximum = MinPoint.Y + Zoom;

		return (plotModel, MinPoint);
	}
}
