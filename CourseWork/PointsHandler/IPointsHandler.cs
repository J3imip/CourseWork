namespace CourseWork;

internal interface IPointsHandler
{
	public void ClearPoints(object sender, System.Windows.RoutedEventArgs e);
	public void RemovePoint(object sender, System.Windows.RoutedEventArgs e);
	public void InsertOrUpdatePoint(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e);
	public void ChangeFunction(object sender, System.Windows.RoutedEventArgs e);
}