using OxyPlot;

namespace CourseWork;

internal interface IGraphPlotter
{
	(PlotModel, Calculations.Point) CreatePlotModel();
}