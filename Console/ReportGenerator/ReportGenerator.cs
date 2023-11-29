using Calculations;

namespace CourseWork;

public class ReportGenerator : IReportGenerator
{
	private DataHandler dh;
	private string ReportFilePath = "";
	private Point MinPoint;

	private string GraphSVG = "";

	public ReportGenerator(DataHandler dh, Point MinPoint) {
		this.dh = dh;
		this.MinPoint = MinPoint;
	}

	private DataModel Data
	{
		get => dh.Data;
		set { dh.Data = value; }
	}

	public void SaveReportAs()
	{
		try
		{
			ReportFilePath = "test.html";
			GenerateReport(GraphSVG);
		} 
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
	private void GenerateReport(string GraphSVG)
	{
		Func<double, double> fx = x => LagrangeInterpolation.GetValue(Data.GetPoints("fx"), x);
		Func<double, double> gx = x => LagrangeInterpolation.GetValue(Data.GetPoints("gx"), x);

		var minFx = Fibonacci.FindMinimum(fx, Data.GetMinimalX("fx"), Data.GetMaximalX("fx"), 10);
		var minGx = Fibonacci.FindMinimum(gx, Data.GetMinimalX("gx"), Data.GetMaximalX("gx"), 10);
		var minGFXPoint = MinPoint;

		string html = "<!DOCTYPE html>\n<html>\n<head>\n<title>Report</title>\n</head>\n<body>\n";
		html += "<h1>Report</h1>\n";
		html += "<h2>Points of f(x):</h2>\n";
		html += "<table>\n";

		foreach (var point in Data.GetPointsMap()["fx"])
		{
			html += "<tr>\n";
			html += $"<td>({point.X}; {point.Y})</td>\n";
			html += "</tr>\n";
		}

		html += "</table>\n";

		html += "<h2>Points of g(x):</h2>\n";
		html += "<table>\n";

		foreach (var point in Data.GetPointsMap()["gx"])
		{
			html += "<tr>\n";
			html += $"<td>({point.X}; {point.Y})</td>\n";
			html += "</tr>\n";
		}

		html += "</table>\n";

		html += $"<h2>Minimum of f(x) using Fibonacci method:</h2>\n";
		html += $"<p>x: ({minFx.ToString("0.000")}; {fx(minFx).ToString("0.000")})</p>\n";

		html += $"<h2>Minimum of g(x) using Fibonacci method:</h2>\n";
		html += $"<p>x: ({minGx.ToString("0.000")}; {gx(minGx).ToString("0.000")})</p>\n";

		html += $"<h2>f(x) - g(x) -> min:</h2>\n";
		html += $"<p>x: ({minGFXPoint!.X:0.000}; {minGFXPoint.Y:0.000})</p>\n";

		html += "<h2>Graph:</h2>\n";
		html += GraphSVG;

		File.WriteAllText(ReportFilePath, html);
	}
}
