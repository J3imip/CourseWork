using Calculations;
using DocumentFormat.OpenXml.Wordprocessing;
using OxyPlot;
using OxyPlot.Annotations;
using System.Windows;
using System.Windows.Input;
using Point = Calculations.Point;

namespace CourseWork
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class GraphWindow : Window
	{
		private double Zoom = 15;
		private double Accuracy = 1e-3;
		private int FibonacciIterations = 20;

		public PlotModel GraphModel { get; set; }
		public DataModel DataModel { get; set; }
		public string GraphSVG { get; set; }
		public Point? MinPoint;
		private GraphPlotter gp;

		public GraphWindow(DataModel dataModel)
		{
			DataModel = dataModel;
			InitializeComponent();

			gp = new(DataModel, Zoom, Accuracy, FibonacciIterations);

			(GraphModel, MinPoint) = CreatePlotModel();
			GraphSVG = GetGraphSvg();

			MinXTextBox.Text = (DataModel.GetMinimalX("fx") - DataModel.GetMinimalX("gx")).ToString();
			MaxXTextBox.Text = (DataModel.GetMaximalX("fx") + DataModel.GetMaximalX("gx")).ToString();

			DataContext = this;
		}
		public string GetGraphSvg()
		{
			var svgExporter = new OxyPlot.Wpf.SvgExporter { Width = 400, Height = 300 };
			return svgExporter.ExportToString(GraphModel);
		}

		private (PlotModel,Point) CreatePlotModel() { return gp.CreatePlotModel(); }
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
				case Key.Enter:
					ApplyLimitsButton_Click(sender, e);
					break;
			}
		}

		private void ApplyLimitsButton_Click(object sender, RoutedEventArgs e)
		{
			MinXTextBox.Text = MinXTextBox.Text.Replace('.', ',');
			MaxXTextBox.Text = MaxXTextBox.Text.Replace('.', ',');

			(GraphModel, MinPoint) = gp.UpdateMinPoint(
				double.Parse(MinXTextBox.Text),
				double.Parse(MaxXTextBox.Text)
			);
			GraphSVG = GetGraphSvg();
			GraphModel.InvalidatePlot(true);
		}
	}
}