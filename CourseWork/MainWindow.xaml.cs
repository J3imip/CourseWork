using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CourseWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataModel dataModel = new DataModel();
		private ObservableCollection<Point> pointViewModels = new();

		string filePath = "";
		string GraphSvg = "";
		GraphWindow graphWindow;

		public MainWindow()
		{
			InitializeComponent();

			DataModel dataModel = new DataModel();

			graphWindow = new GraphWindow(dataModel);
			GraphSvg = graphWindow.GetGraphSvg();
			pointsGrid.ItemsSource = pointViewModels;
		}
		private void Clear_Click(object sender, RoutedEventArgs e)
		{
			var functionChosen = fxRadioButton.IsChecked.GetValueOrDefault() ? "fx" : "gx";

			dataModel.ClearPoints(functionChosen);
			pointViewModels.Clear();
			pointsGrid.ItemsSource = pointViewModels;
		}
		private void RemovePoint_Click(object sender, RoutedEventArgs e)
		{
			var functionChosen = fxRadioButton.IsChecked.GetValueOrDefault() ? "fx" : "gx";

			if (pointsGrid.SelectedItem != null)
			{
				var selectedPoint = (Point)pointsGrid.SelectedItem;
				pointViewModels.Remove(selectedPoint);
				pointsGrid.ItemsSource = pointViewModels;
				dataModel.RemovePoint(selectedPoint, functionChosen);
			}
		}

		private void fxRadioButton_Click(object sender, RoutedEventArgs e)
		{
			pointViewModels = new ObservableCollection<Point>(dataModel.GetPoints("fx"));
			pointsGrid.ItemsSource = pointViewModels;
		}

		private void gxRadioButton_Click(object sender, RoutedEventArgs e)
		{
			pointViewModels = new ObservableCollection<Point>(dataModel.GetPoints("gx"));
			pointsGrid.ItemsSource = pointViewModels;
		}
		private void SaveDatasetAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "XML files (*.xml)|*.xml";
			if (saveFileDialog.ShowDialog() == true)
			{
				filePath = saveFileDialog.FileName;
				XmlHandler serializer = new XmlHandler();
				serializer.SerializeDictionary(dataModel.GetPointsMap(), filePath);

				MessageBox.Show("Dataset saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
		private void SaveDataset_Click(object sender, RoutedEventArgs e) {
			XmlHandler serializer = new XmlHandler();
			serializer.SerializeDictionary(dataModel.GetPointsMap(), filePath);

			MessageBox.Show("Dataset saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void OpenDataset_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "XML files (*.xml)|*.xml";
			if (openFileDialog.ShowDialog() == true)
			{
				filePath = openFileDialog.FileName;
				XmlHandler serializer = new XmlHandler();
				dataModel.SetPointsMap(serializer.DeserializeDictionary(filePath));
				graphWindow = new GraphWindow(dataModel);
				GraphSvg = graphWindow.GetGraphSvg();

				var functionChosen = fxRadioButton.IsChecked.GetValueOrDefault() ? "fx" : "gx";
				pointViewModels = new ObservableCollection<Point>(dataModel.GetPoints(functionChosen));
				pointsGrid.ItemsSource = pointViewModels;
			}
		}

		private void NewDataset_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "XML files (*.xml)|*.xml";
			if (saveFileDialog.ShowDialog() == true)
			{
				filePath = saveFileDialog.FileName;
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
			{
				SaveDataset_Click(sender, e);
			}
		}
		
		private void Calculate_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Func<double, double> f = x => LagrangeInterpolation.GetValue(dataModel.GetPoints("fx"), x) - LagrangeInterpolation.GetValue(dataModel.GetPoints("gx"), x);

				var minX = FunctionCalculator.FindMinimum(
					f,
					dataModel.GetMinimalX("fx") - dataModel.GetMinimalX("gx"),
					dataModel.GetMaximalX("fx") + dataModel.GetMaximalX("gx"),
					20,
					1e-3
				);
				var minY = f(minX);

				MessageBox.Show($"x: {minX.ToString("0.000")}\ny: {minY.ToString("0.000")}", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception error) {
				MessageBox.Show("Error occured while calculating the result. Please check if you have entered all the points correctly. " + error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ShowGraph_Click(object sender, RoutedEventArgs e)
		{
			graphWindow = new GraphWindow(dataModel);
			GraphSvg = graphWindow.GetGraphSvg();
			graphWindow.Show();
		}

		private void ExportDatasetAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "HTML files (*.html)|*.html";

			if (saveFileDialog.ShowDialog() == true)
			{
				ReportGenerator.GenerateReport(dataModel, GraphSvg, saveFileDialog.FileName);

				MessageBox.Show("Report exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			Application.Current.Shutdown();
		}

		private void pointsGrid_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e)
		{
			var functionChosen = fxRadioButton.IsChecked.GetValueOrDefault() ? "fx" : "gx";

			var point = (Point)e.Row.Item;
			dataModel.RemovePoint(point, functionChosen);
			dataModel.AddPoint(functionChosen, point);
		}
	}
}
