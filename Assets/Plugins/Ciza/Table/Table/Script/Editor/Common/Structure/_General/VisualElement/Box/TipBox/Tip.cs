namespace CizaTable.Editor.TipBoxVisual
{
	public struct Tip
	{
		public string Title { get; }
		public string Description { get; }

		public Tip(string title, string description)
		{
			Title = title;
			Description = description;
		}
	}
}