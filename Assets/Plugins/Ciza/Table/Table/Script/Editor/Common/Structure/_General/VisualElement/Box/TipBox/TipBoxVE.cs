using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor.TipBoxVisual
{
	public class TipBoxVE : VisualElement
	{
		// VARIABLE: -----------------------------------------------------------------------------
		protected readonly List<TipVE> TipVEs = new List<TipVE>();

		protected readonly VisualElement Head = new VisualElement();
		protected readonly VisualElement Body = new VisualElement();

		protected virtual string[] USSPaths => new[] { "TipBox" };

		protected virtual string[] RootClasses => new[] { "tipbox-root" };
		protected virtual string[] HeadClasses => new[] { "tipbox-head" };
		protected virtual string[] BodyClasses => new[] { "tipbox-body" };

		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		public bool IsInitialized { get; private set; }

		public string AttachObjName { get; protected set; }
		public string Title { get; protected set; }

		public bool IsExpanded
		{
			get => EditorPrefs.GetBool($"{AttachObjName}.TipBoxVE.{Title}", true);
			set => EditorPrefs.SetBool($"{AttachObjName}.TipBoxVE.{Title}", value);
		}

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public TipBoxVE()
		{
			Add(Head);
			Add(Body);
		}

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public void Initialize(string attachObjName, string title, Tip[] tips)
		{
			if (IsInitialized)
				return;
			IsInitialized = true;

			foreach (var sheet in StyleSheetUtils.GetStyleSheets(USSPaths))
				styleSheets.Add(sheet);

			DerivedInitialize(attachObjName, title, tips);
		}

		public virtual void Refresh(Tip[] tips)
		{
			Body.Clear();

			for (int i = 0; i < tips.Length; i++)
			{
				if (i >= TipVEs.Count)
					TipVEs.Add(CreateTipVE(tips[i]));

				TipVEs[i].Refresh(tips[i]);
				Body.Add(TipVEs[i]);
			}

			RefreshBody();
		}

		// PROTECT METHOD: --------------------------------------------------------------------

		protected virtual void DerivedInitialize(string attachObjName, string title, Tip[] tips)
		{
			foreach (var rootClass in RootClasses)
				AddToClassList(rootClass);

			foreach (var headClass in HeadClasses)
				Head.AddToClassList(headClass);

			foreach (var bodyClass in BodyClasses)
				Body.AddToClassList(bodyClass);

			AttachObjName = attachObjName;
			Title = title;
			var button = new Button(() =>
			{
				IsExpanded = !IsExpanded;
				RefreshBody();
			}) { text = Title };
			Head.Add(button);

			Refresh(tips);
		}


		protected virtual TipVE CreateTipVE(Tip tip)
		{
			var tipVE = new TipVE();
			tipVE.Initialize(tip);
			return tipVE;
		}

		protected virtual void RefreshBody()
		{
			Body.style.display = IsExpanded ? DisplayStyle.Flex : DisplayStyle.None;
		}
	}
}