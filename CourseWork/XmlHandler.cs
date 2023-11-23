using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CourseWork
{
	internal class XmlHandler
	{
		public void SerializeDictionary(Dictionary<string, List<Point>> dictionary, string filePath)
		{
			try
			{
				var serializableData = new SerializableData
				{
					Entries = new List<DataEntry>()
				};

				foreach (var kvp in dictionary)
				{
					var dataEntry = new DataEntry
					{
						Key = kvp.Key,
						Points = kvp.Value
					};
					serializableData.Entries.Add(dataEntry);
				}

				XmlSerializer serializer = new XmlSerializer(typeof(SerializableData));
				using (StreamWriter writer = new StreamWriter(filePath))
				{
					serializer.Serialize(writer, serializableData);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurred while saving data to XML: {ex.Message}");
			}
		}
		public Dictionary<string, List<Point>> DeserializeDictionary(string filePath)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(SerializableData));
				using (StreamReader reader = new StreamReader(filePath))
				{
					var serializableData = (SerializableData)serializer.Deserialize(reader);
					var dictionary = new Dictionary<string, List<Point>>();
					foreach (var entry in serializableData.Entries)
					{
						dictionary.Add(entry.Key, entry.Points);
					}
					return dictionary;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occurred while loading data from XML: {ex.Message}");
				return new Dictionary<string, List<Point>>();
			}
		}
	}
}
