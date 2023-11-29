namespace CourseWork;

public class DataHandler : IDataHandler
{
	internal string FilePath { get; set; } = "";
	internal string FunctionChosen { get; set; } = "fx";
	internal DataModel Data { get; set; } = new();


	public DataHandler() { }
}
