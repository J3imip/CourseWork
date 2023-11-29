using Calculations;
using System.Collections.Generic;

namespace CourseWork
{
	internal interface IXmlHandler
	{
		void SerializeDictionary(Dictionary<string, List<Point>> dictionary, string filePath);
		Dictionary<string, List<Point>> DeserializeDictionary(string filePath);
	}
}