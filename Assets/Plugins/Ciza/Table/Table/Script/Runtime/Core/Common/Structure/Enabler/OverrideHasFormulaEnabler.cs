using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public class OverrideHasFormulaEnabler : BEnabler<OverrideHasFormula>
	{
		// VARIABLE: -----------------------------------------------------------------------------

		[SerializeField]
		protected OverrideHasFormula _value;

		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		protected override OverrideHasFormula ValueImp
		{
			get => _value;
			set => _value = value;
		}

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public OverrideHasFormulaEnabler() { }
		
		[Preserve]
		public OverrideHasFormulaEnabler(bool isEnable) =>
			_isEnable = isEnable;

		[Preserve]
		public OverrideHasFormulaEnabler(OverrideHasFormula value) : base(value) { }

		[Preserve]
		public OverrideHasFormulaEnabler(bool isEnable, OverrideHasFormula value) : base(isEnable, value) { }
	}

	[Serializable]
	public class OverrideHasFormula
	{
		[SerializeField]
		protected StringEnabler _hasFormula;

		public virtual bool TryGetFormulaDataId(out string formulaDataId) =>
			_hasFormula.TryGetValue(out formulaDataId);
	}
}