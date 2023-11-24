using System.Collections.ObjectModel;
using System.Windows.Controls;
using Calculations;

namespace CourseWork;

public class PointsHandler : IPointsHandler
{
	DataHandler dh;
	RadioButton FunctionsRadioButton;

	private DataModel Data
	{
		get => dh.Data;
		set { dh.Data = value; }
	}
	private string FunctionChosen
	{
		get => dh.FunctionChosen;
		set { dh.FunctionChosen = value; }
	}
	private ObservableCollection<Point>? PointViewModels
	{
		get => dh.PointViewModels;
		set { dh.PointViewModels = value; }
	}
	private DataGrid? PointsGrid
	{
		get => dh.PointsGrid;
		set { dh.PointsGrid = value; }
	}

	public PointsHandler(
		ref DataHandler dh, 
		RadioButton FunctionsRadioButton
	) {
		this.dh = dh;
		this.FunctionsRadioButton = FunctionsRadioButton;
	}
	public void ChangeFunction(object sender, System.Windows.RoutedEventArgs e)
	{
		FunctionChosen = FunctionsRadioButton.IsChecked.GetValueOrDefault() ? "fx" : "gx";

		PointViewModels = new ObservableCollection<Point>(Data.GetPoints(FunctionChosen));
		PointsGrid!.ItemsSource = PointViewModels;
	}

	public void ClearPoints(object sender, System.Windows.RoutedEventArgs e)
	{
		Data.ClearPoints(FunctionChosen);
		PointViewModels!.Clear();
		PointsGrid!.ItemsSource = PointViewModels;
	}

	public void InsertOrUpdatePoint(object sender, DataGridRowEditEndingEventArgs e)
	{
		var point = (Point)e.Row.Item;
		Data.RemovePoint(point, FunctionChosen);
		Data.AddPoint(FunctionChosen, point);
	}

	public void RemovePoint(object sender, System.Windows.RoutedEventArgs e)
	{
		if (PointsGrid!.SelectedItem != null)
		{
			var selectedPoint = (Point)PointsGrid.SelectedItem;

			Data.RemovePoint(selectedPoint, FunctionChosen);
			PointViewModels.Remove(selectedPoint);
			PointsGrid.ItemsSource = PointViewModels;
		}
	}
}
