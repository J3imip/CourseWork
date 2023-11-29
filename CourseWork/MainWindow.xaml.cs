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
		public MainWindow()
		{
			InitializeComponent();

			dh = new(
				ref PointViewModels, 
				ref pointsGrid
			);

			ph = new(
				ref dh,
				fxRadioButton
			);
		}

		private DataHandler dh;
		private PointsHandler ph;
		private DataModel Data
		{
			get => dh.Data;
			set { dh.Data = value; }
		}

		private ObservableCollection<Calculations.Point> PointViewModels = new();

		private void Clear_Click(object sender, RoutedEventArgs e) { ph.ClearPoints(sender, e); }
		private void RemovePoint_Click(object sender, RoutedEventArgs e) { ph.RemovePoint(sender, e); }
		private void pointsGrid_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e) { ph.InsertOrUpdatePoint(sender, e); }
		private void ChangeFunction_Click(object sender, RoutedEventArgs e) { ph.ChangeFunction(sender, e); }

		private void SaveDatasetAs_Click(object sender, RoutedEventArgs e) { dh.SaveDataAs(); }
		private void SaveDataset_Click(object sender, RoutedEventArgs e) { dh.SaveData(); } 
		private void NewDataset_Click(object sender, RoutedEventArgs e) { dh.CreateFile(); }
		private void OpenDataset_Click(object sender, RoutedEventArgs e) { dh.LoadData(); }

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control) { dh.SaveData(); }
		}

		private void Calculate_Click(object sender, RoutedEventArgs e)
		{
			CalculationHandler ch = new(dh);
			ch.Calculate(sender, e);
		}

		private void ShowGraph_Click(object sender, RoutedEventArgs e)
		{
			GraphWindow gw = new(Data);
			gw.Show();
		}

		private void ExportDatasetAs_Click(object sender, RoutedEventArgs e)
		{
			ReportGenerator rg = new(dh);
			rg.SaveReportAs();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Application.Current.Shutdown();
		}

		private void Help_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(
				"1. To add a point, click on the last row of the table and enter the values of x and y.\n" +
				"2. To remove a point, select it and click the \"Remove point\" button.\n" +
				"3. To clear the table, click the \"Clear\" button.\n" +
				"\nGraph controls:\n" +
				"R - reset zoom\n",
				"Help",
				MessageBoxButton.OK,
				MessageBoxImage.Information
			);
		}
	}
}
