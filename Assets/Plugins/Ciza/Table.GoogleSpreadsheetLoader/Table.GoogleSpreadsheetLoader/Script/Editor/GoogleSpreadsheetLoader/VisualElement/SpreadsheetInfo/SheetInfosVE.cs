using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class SheetInfosVE : ListVE
	{
		[Preserve]
		public SheetInfosVE(SerializedProperty listProperty) : base(listProperty) { }

		protected override void DerivedInitialize()
		{
			base.DerivedInitialize();
			this.SetMargin(3, -2, 0, 0);
			Refresh();
		}
		
		protected override void SetupHead()
		{
			_head.Add(CreateHeadLabel("Sheet Id", 50));
			_head.Add(CreateHeadLabel("Name", 40));
			var isUsingLabel = CreateHeadLabel("Is Using", 10, true);
			isUsingLabel.style.minWidth = 60;
			_head.Add(isUsingLabel);
			_head.style.marginBottom = 0;
			_head.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
		}

		protected override void SetupFoot()
		{
			_foot.style.height = 0;
			_foot.SetMargin(0);
		}

		protected override ItemVE CreateItemVE(SerializedProperty itemProperty)
		{
			var sheetInfoVE = new SheetInfoVE(this, itemProperty);
			sheetInfoVE.Initialize();
			return sheetInfoVE;
		}

		protected virtual Label CreateHeadLabel(string text, float widthPercentage, bool isLast = false)
		{
			var label = new Label(text);
			label.style.flexGrow = 1;
			label.style.flexShrink = 1;
			label.style.overflow = Overflow.Hidden;
			label.style.unityTextAlign = TextAnchor.MiddleCenter;
			label.style.width = Length.Percent(widthPercentage);
			label.SetBorder(1, Color.black, isLast ? SideKinds.All : SideKinds.NoRight);
			return label;
		}
	}
}