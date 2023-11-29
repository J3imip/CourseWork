using Calculations;
using System.Collections.Generic;

namespace CourseWork;

public class DataModel : IDataModel
{
	private Dictionary<string, List<Point>> pointsMap = new Dictionary<string, List<Point>>();

	public void ClearPoints(string functionName)
	{
		if (pointsMap.ContainsKey(functionName))
		{
			pointsMap[functionName].Clear();
		}
	}

	public double GetMinimalX(string functionName)
	{
		if (pointsMap.ContainsKey(functionName))
		{
			double minX = pointsMap[functionName][0].X;
			foreach (var point in pointsMap[functionName])
			{
				if (point.X < minX)
				{
					minX = point.X;
				}
			}

			return minX;
		}

		return 0;
	}

	public double GetMinimalY(string functionName)
	{
		if (pointsMap.ContainsKey(functionName))
		{
			double minY = pointsMap[functionName][0].Y;
			foreach (var point in pointsMap[functionName])
			{
				if (point.Y < minY)
				{
					minY = point.Y;
				}
			}

			return minY;
		}

		return 0;
	}
	public double GetMaximalX(string functionName)
	{
		if (pointsMap.ContainsKey(functionName))
		{
			double maxX = pointsMap[functionName][0].X;
			foreach (var point in pointsMap[functionName])
			{
				if (point.X > maxX)
				{
					maxX = point.X;
				}
			}

			return maxX;
		}

		return 0;
	}
	public double GetMaximalY(string functionName)
	{
		if (pointsMap.ContainsKey(functionName))
		{
			double maxY = pointsMap[functionName][0].Y;
			foreach (var point in pointsMap[functionName])
			{
				if (point.Y > maxY)
				{
					maxY = point.Y;
				}
			}

			return maxY;
		}

		return 0;
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

	public void RemovePoint(Point point, string func)
	{
		if (pointsMap.ContainsKey(func))
		{
			pointsMap[func].Remove(point);
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

public class SerializableData
{
	public required List<DataEntry> Entries { get; set; }
}

public class DataEntry
{
	public required string Key { get; set; }
	public required List<Point> Points { get; set; }
}