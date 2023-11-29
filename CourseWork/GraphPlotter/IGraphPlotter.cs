using OxyPlot;

namespace CourseWork;

internal interface IGraphPlotter
{
	(PlotModel, Calculations.Point) CreatePlotModel();
	(PlotModel, Calculations.Point) UpdateMinPoint(double xMin, double xMax);
}