using Calculations;
using CourseWork;

namespace ConsoleTest;

class Program
{
	public static void Main()
	{
		XmlHandler xmlHandler = new();

		Dictionary<string, List<Point>> dictionary = new()
		{
			{ "fx", new List<Point> { new Point(2, 4), new Point(0, 0), new Point(3, 9) } },
			{ "gx", new List<Point> { new Point(0, 2), new Point(4, 2) } },
		};

		xmlHandler.SerializeDictionary(dictionary, "test.xml");
		Dictionary<string, List<Point>> dictionary2 = xmlHandler.DeserializeDictionary("test.xml");
		foreach (var kvp in dictionary2)
		{
			Console.WriteLine(kvp.Key + ":");
			foreach (var point in kvp.Value)
			{
				Console.WriteLine(point);
			}
		}

		DataModel dataModel = new();
		dataModel.SetPointsMap(dictionary);
		Console.WriteLine($"Min f(x) = ({dataModel.GetMinimalX("fx")};{dataModel.GetMinimalY("fx")})");
		Console.WriteLine($"Max f(x) = ({dataModel.GetMaximalX("fx")};{dataModel.GetMaximalY("fx")})");
		Console.WriteLine($"Min g(x) = ({dataModel.GetMinimalX("gx")};{dataModel.GetMinimalY("gx")})");
		Console.WriteLine($"Mix g(x) = ({dataModel.GetMaximalX("gx")};{dataModel.GetMaximalY("gx")})");


		double x = 2;
		double y = LagrangeInterpolation.GetValue(dataModel.GetPoints("fx"), x);

		Console.WriteLine($"LagrangeInterpolation.GetValue({x}) = ({x:0.000};{y:0.000})");


		double a = dataModel.GetMinimalX("fx") - dataModel.GetMinimalX("gx");
		double b = dataModel.GetMaximalX("fx") + dataModel.GetMaximalX("gx");
		int iter = 20;

		Func<double, double> f = x => 
			LagrangeInterpolation.GetValue(dataModel.GetPoints("fx"), x) - 
			LagrangeInterpolation.GetValue(dataModel.GetPoints("gx"), x);

		double minimum = Fibonacci.FindMinimum(
			f, 
			a, 
			b, 
			iter
		);
		Console.WriteLine($"Minimum [{a}, {b}] is {minimum:0.000}; {f(minimum):0.000}");

		{
			DataHandler dataHandler = new();
			dataHandler.Data.SetPointsMap(dictionary);
			ReportGenerator reportGenerator = new(dataHandler, new(minimum, f(minimum)));
			reportGenerator.SaveReportAs();
		}
	}
}