using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows;

namespace CourseWork
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class GraphWindow : Window
	{
		public GraphWindow(DataModel dataModel)
		{
			this.dataModel = dataModel;
			InitializeComponent();
			GraphModel = CreatePlotModel();

			DataContext = this;
		}

		public PlotModel GraphModel { get; set; }
		public DataModel dataModel { get; set; }

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

			plotModel.Series.Add(new FunctionSeries(x => LagrangeInterpolation.GetValue(dataModel.GetPoints(), x), -10, 10, 0.001));

			var minX = FunctionCalculator.FindMinimum(
				dataModel,
				dataModel.GetMinimalX(), 
				dataModel.GetMaximalX(), 
				10
			);
			var minY = LagrangeInterpolation.GetValue(dataModel.GetPoints(), minX);

			plotModel.Annotations.Add(new PointAnnotation { X = minX, Y = minY, Text = $"({minX.ToString("0.000")};{minY.ToString("0.000")})" });
			return plotModel;
		}
	}
}