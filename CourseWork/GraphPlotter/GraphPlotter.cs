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
	private double Zoom = 10;
	private double Accuracy = 1e-3;
	private int FibonacciIterations = 20;
	private DataModel dm;
	private Point? MinPoint;

	public GraphPlotter(
		DataModel dm
	)
	{
		this.dm = dm;
	}

	public (PlotModel, Point) CreatePlotModel()
	{
		var plotModel = new PlotModel();

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

		Func<double, double> f = x =>
			LagrangeInterpolation.GetValue(dm.GetPoints("fx"), x) -
			LagrangeInterpolation.GetValue(dm.GetPoints("gx"), x);

		plotModel.Series.Add(
			new FunctionSeries(
				f,
				dm.GetMinimalX("fx") - dm.GetMinimalX("gx") - Zoom,
				dm.GetMaximalX("fx") + dm.GetMaximalX("gx") + Zoom,
				Accuracy
			)
		);

		var minX = Fibonacci.FindMinimum(
			f,
			dm.GetMinimalX("fx") - dm.GetMinimalX("gx"),
			dm.GetMaximalX("fx") + dm.GetMaximalX("gx"),
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
}
