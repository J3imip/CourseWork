using System;
using System.IO;

namespace CourseWork
{
	static public class ReportGenerator
	{
		static public void GenerateReport(DataModel dataModel, string graphSvg, string filePath)
		{

			Func<double, double> fx = x => LagrangeInterpolation.GetValue(dataModel.GetPoints("fx"), x);
			Func<double, double> gx = x => LagrangeInterpolation.GetValue(dataModel.GetPoints("gx"), x);

			var minFx = FunctionCalculator.FindMinimum(fx, dataModel.GetMinimalX("fx"), dataModel.GetMaximalX("fx"), 10);
			var minGx = FunctionCalculator.FindMinimum(gx, dataModel.GetMinimalX("gx"), dataModel.GetMaximalX("gx"), 10);
			var minFGX = FunctionCalculator.FindMinimum(
				x => fx(x) - gx(x),
				dataModel.GetMinimalX("fx") - dataModel.GetMinimalX("gx"), 
				dataModel.GetMaximalX("fx") + dataModel.GetMaximalX("gx"),
			10);

			string html = "<!DOCTYPE html>\n<html>\n<head>\n<title>Report</title>\n</head>\n<body>\n";
			html += "<h1>Report</h1>\n";
			html += "<h2>Points of f(x):</h2>\n";
			html += "<table>\n";

			foreach (var point in dataModel.GetPointsMap()["fx"])
			{
				html += "<tr>\n";
				html += $"<td>({point.X}; {point.Y})</td>\n";
				html += "</tr>\n";
			}

			html += "</table>\n";

			html += "<h2>Points of g(x):</h2>\n";
			html += "<table>\n";

			foreach (var point in dataModel.GetPointsMap()["gx"])
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
			html += $"<p>x: ({minFGX.ToString("0.000")}; {(fx(minFGX) - gx(minFGX)).ToString("0.000")})</p>\n";

			html += "<h2>Graph:</h2>\n";
			html += graphSvg;

			File.WriteAllText(filePath, html);
		}
	}
}
