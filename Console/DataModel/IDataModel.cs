using Calculations;
using System.Collections.Generic;

namespace CourseWork
{
	public interface IDataModel
	{
		void ClearPoints(string functionName);
		double GetMinimalX(string functionName);
		double GetMinimalY(string functionName);
		double GetMaximalX(string functionName);
		double GetMaximalY(string functionName);
		void AddPoint(string functionName, Point point);
		List<Point> GetPoints(string functionName);
	}
}