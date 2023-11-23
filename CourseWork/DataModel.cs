using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CourseWork
{
	public class DataModel
	{
		private Dictionary<string, List<Point>> pointsMap = new Dictionary<string, List<Point>>();

		public double GetMinimalX()
		{
			double minX = double.MaxValue;

			List<Point> differencePoints = GetPoints();

			foreach (var point in differencePoints)
			{
				if (point.X < minX)
				{
					minX = point.X;
				}
			}

			return minX;
		}

		public double GetMaximalX()
		{
			double maxX = double.MinValue;

			List<Point> differencePoints = GetPoints();

			foreach (var point in differencePoints)
			{
				if (point.X > maxX)
				{
					maxX = point.X;
				}
			}

			return maxX;
		}

		public void SetPointsMap(Dictionary<string, List<Point>> pointsMap)
		{
			this.pointsMap = pointsMap;
		}

		public void AddPoint(string functionName, Point point)
		{
			if (!pointsMap.ContainsKey(functionName))
			{
				pointsMap[functionName] = new List<Point>();
			}
			pointsMap[functionName].Add(point);
		}

		public Dictionary<string, List<Point>> GetPointsMap()
		{
			return pointsMap;
		}
		public List<Point> GetPoints(string functionName)
		{
			if (pointsMap.ContainsKey(functionName))
			{
				return pointsMap[functionName];
			}
			return new List<Point>();
		}

		public List<Point> GetPoints()
		{
			List<Point> differencePoints = new List<Point>();

			// Проверяем, что у нас есть одинаковое количество точек для обеих функций
			if (pointsMap.ContainsKey("fx") && pointsMap.ContainsKey("gx") &&
				pointsMap["fx"].Count == pointsMap["gx"].Count)
			{
				for (int i = 0; i < pointsMap["fx"].Count; i++)
				{
					double differenceX = pointsMap["fx"][i].X; // Берем значение X из функции fx
					double differenceY = pointsMap["fx"][i].Y - pointsMap["gx"][i].Y; // Вычитаем значение Y функции gx из значения Y функции fx

					Point differencePoint = new Point(differenceX, differenceY);
					differencePoints.Add(differencePoint);
				}
			}
			else
			{
				Console.WriteLine("Количество точек для функций fx и gx не совпадает или функции не заданы.");
			}

			return differencePoints;
		}

		public void Clear(string functionName)
		{
			if (pointsMap.ContainsKey(functionName))
			{
				pointsMap[functionName].Clear();
			}
		}

		public void RemoveLastPoint(string functionName)
		{
			if (pointsMap.ContainsKey(functionName) && pointsMap[functionName].Count > 0)
			{
				pointsMap[functionName].RemoveAt(pointsMap[functionName].Count - 1);
			}
		}


		public override string ToString()
		{

			List<string> result = new List<string>();
			foreach (var pair in pointsMap)
			{
				result.Add($"{pair.Key}:\n{string.Join("\n", pair.Value)}");
			}
			return string.Join("\n\n", result);
		}

		public string ToString(string functionName)
		{
			if (pointsMap.ContainsKey(functionName))
			{
				return string.Join("\n", pointsMap[functionName]);
			}
			return "";
		}

		public DataModel() { }
	}

	public class Point
	{
		public double X { get; set; }
		public double Y { get; set; }

		override public string ToString()
		{
			return $"Point: ({X}, {Y})";
		}

		public Point() { }
		public Point(double x, double y) {
			X = x;
			Y = y;
		}
	}

	public class SerializableData
	{
		public List<DataEntry> Entries { get; set; }
	}

	public class DataEntry
	{
		public string Key { get; set; }
		public List<Point> Points { get; set; }
	}

}