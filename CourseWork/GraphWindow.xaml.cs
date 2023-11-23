using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace CourseWork
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class GraphWindow : Window
	{
		public GraphWindow(DataModel dataModel)
		{
			DataModel = dataModel;
			InitializeComponent();
			GraphModel = CreatePlotModel();

			DataContext = this;
		}

		public string GetGraphSvg()
		{
			var svgExporter = new OxyPlot.Wpf.SvgExporter { Width = 400, Height = 300 };
			return svgExporter.ExportToString(GraphModel);
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					Close();
					break;
				case Key.R:
					//focus at the minimum point
					GraphModel.Axes[0].Reset();
					GraphModel.Axes[1].Reset();

					GraphModel.InvalidatePlot(true);
					break;		
			}
		}

		public PlotModel GraphModel { get; set; }
		public DataModel DataModel { get; set; }
		private Point MinPoint { get; set; }

		private PlotModel CreatePlotModel()
		{
			var plotModel = new PlotModel();

			// Adding a horizontal axis (X-axis) at the middle of the screen
			plotModel.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Bottom,
				Minimum = -10,
				Maximum = 10,
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
				Minimum = -10,
				Maximum = 10,
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

			Func<double, double> f = x => LagrangeInterpolation.GetValue(DataModel.GetPoints("fx"), x) - LagrangeInterpolation.GetValue(DataModel.GetPoints("gx"), x);

			plotModel.Series.Add(
				new FunctionSeries(
				f,
				DataModel.GetMinimalX("fx") - DataModel.GetMinimalX("gx") - 10.0,
				DataModel.GetMaximalX("fx") + DataModel.GetMaximalX("gx") + 10.0,
				1e-3
				)
			);

			var minX = FunctionCalculator.FindMinimum(
				f,
				DataModel.GetMinimalX("fx") - DataModel.GetMinimalX("gx"),
				DataModel.GetMaximalX("fx") + DataModel.GetMaximalX("gx"),
				20,
				1e-3
			);
			var minY = f(minX);
			MinPoint = new Point(minX, minY);

			plotModel.Annotations.Add(new PointAnnotation { X = minX, Y = minY, Text = $"({minX.ToString("0.000")};{minY.ToString("0.000")})" });

			//focus at the minimum point
			plotModel.Axes[0].Minimum = minX - 10;
			plotModel.Axes[0].Maximum = minX + 10;
			plotModel.Axes[1].Minimum = minY - 10;
			plotModel.Axes[1].Maximum = minY + 10;

			return plotModel;
		}
	}
}