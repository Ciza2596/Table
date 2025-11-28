using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class SpreadsheetContentInfosVE : ListVE
	{
		public override bool IsAllowReordering => false;
		public override bool IsAllowDuplicate => false;
		public override bool IsAllowDelete => false;


		[Preserve]
		public SpreadsheetContentInfosVE(SerializedProperty listProperty, bool isAutoRefresh) : base(listProperty, isAutoRefresh) { }

		protected override ItemVE CreateItemVE(SerializedProperty itemProperty)
		{
			var spreadsheetContentVE = new SpreadsheetContentInfoVE(this, itemProperty);
			spreadsheetContentVE.Initialize();
			return spreadsheetContentVE;
		}

		protected override void SetupFoot()
		{
			_foot.style.height = 0;
			_foot.SetMargin(0);
		}
	}
}