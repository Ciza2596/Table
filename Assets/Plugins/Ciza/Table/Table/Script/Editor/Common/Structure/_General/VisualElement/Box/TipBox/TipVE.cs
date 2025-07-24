using System;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor.TipBoxVisual
{
	public class TipVE : VisualElement
	{
		// VARIABLE: -----------------------------------------------------------------------------

		[NonSerialized]
		protected readonly TextField TitleLabel = new TextField() { isReadOnly = true };

		[NonSerialized]
		protected readonly Label DescriptionLabel = new Label();

		protected virtual string[] TitleClasses => new[] { "tip-title" };
		protected virtual string[] DescriptionClasses => new[] { "tip-description" };

		protected Tip Tip { get; set; }

		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		public bool IsInitialized { get; private set; }

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public TipVE() { }

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public void Initialize(Tip tip)
		{
			if (IsInitialized)
				return;
			IsInitialized = true;

			Add(TitleLabel);
			Add(DescriptionLabel);

			DerivedInitialize(tip);
		}


		public virtual void Refresh(Tip tip)
		{
			Tip = tip;

			TitleLabel.SetValueWithoutNotify(Tip.Title);
			DescriptionLabel.text = Tip.Description;
		}


		// PROTECT METHOD: --------------------------------------------------------------------

		protected virtual void DerivedInitialize(Tip tip)
		{
			foreach (var titleClass in TitleClasses)
				TitleLabel.AddToClassList(titleClass);

			foreach (var descriptionClasses in DescriptionClasses)
				DescriptionLabel.AddToClassList(descriptionClasses);

			Refresh(tip);
		}
	}
}