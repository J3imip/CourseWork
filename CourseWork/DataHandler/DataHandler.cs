using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Calculations;
using System.Windows;

namespace CourseWork;

public class DataHandler : IDataHandler
{
	internal string FilePath { get; set; } = "";
	internal string FunctionChosen { get; set; } = "fx";
	internal DataModel Data { get; set; } = new();

	internal ObservableCollection<Calculations.Point>? PointViewModels;
	internal DataGrid? PointsGrid;

	public DataHandler() { }

	public DataHandler(ref ObservableCollection<Calculations.Point> PointViewModels, ref DataGrid PointsGrid) {
		this.PointsGrid = PointsGrid;
		this.PointsGrid.ItemsSource = PointViewModels;
		this.PointViewModels = PointViewModels;
	}

	public void LoadData()
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "XML files (*.xml)|*.xml";
			if (openFileDialog.ShowDialog() == true)
			{
				FilePath = openFileDialog.FileName;
				XmlHandler serializer = new XmlHandler();
				Data.SetPointsMap(serializer.DeserializeDictionary(FilePath));
			}

			PointViewModels!.Clear();
			PointViewModels = new ObservableCollection<Calculations.Point>(Data.GetPoints(FunctionChosen));
			PointsGrid!.ItemsSource = PointViewModels;
		}
		catch (System.Exception e)
		{
			MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public void SaveData()
	{
		try
		{
			XmlHandler serializer = new XmlHandler();
			serializer.SerializeDictionary(Data.GetPointsMap(), FilePath);

			MessageBox.Show("Dataset saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}
		catch (System.Exception e)
		{
			MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public void SaveDataAs()
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "XML files (*.xml)|*.xml";
			if (saveFileDialog.ShowDialog() == true)
			{
				FilePath = saveFileDialog.FileName;
				XmlHandler serializer = new XmlHandler();
				serializer.SerializeDictionary(Data.GetPointsMap(), FilePath);

				MessageBox.Show("Dataset saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
		catch (System.Exception e)
		{
			MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public void CreateFile()
	{
		try {
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "XML files (*.xml)|*.xml";
			if (saveFileDialog.ShowDialog() == true)
			{
				FilePath = saveFileDialog.FileName;
			}
		} 
		catch (System.Exception e)
		{
			MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
