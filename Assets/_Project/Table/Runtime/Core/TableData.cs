namespace CizaTable
{
	public abstract class TableData
	{
		//constructor
		protected TableData(string key) => Key = key;

		//public variable
		public string Key { get; }
	}
}
