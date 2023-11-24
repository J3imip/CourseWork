using OxyPlot;
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
		public PlotModel GraphModel { get; set; }
		public DataModel DataModel { get; set; }
		public string GraphSVG { get; set; }
		public Point? MinPoint;
		private GraphPlotter gp;

		public GraphWindow(DataModel dataModel)
		{
			DataModel = dataModel;
			InitializeComponent();

			gp = new(DataModel);

			(GraphModel, MinPoint) = CreatePlotModel();
			GraphSVG = GetGraphSvg();

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
			}
		}
	}
}