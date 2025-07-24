using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public class OverrideHasUIDataIdEnabler : BEnabler<OverrideHasUIDataId>
	{
		// VARIABLE: -----------------------------------------------------------------------------

		[SerializeField]
		protected OverrideHasUIDataId _value;

		protected override OverrideHasUIDataId ValueImp
		{
			get => _value;
			set => _value = value;
		}

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public OverrideHasUIDataIdEnabler() { }

		[Preserve]
		public OverrideHasUIDataIdEnabler(bool isEnable) =>
			_isEnable = isEnable;

		[Preserve]
		public OverrideHasUIDataIdEnabler(OverrideHasUIDataId value) : base(value) { }

		[Preserve]
		public OverrideHasUIDataIdEnabler(bool isEnable, OverrideHasUIDataId value) : base(isEnable, value) { }
	}

	[Serializable]
	public class OverrideHasUIDataId
	{
		[SerializeField]
		protected StringEnabler _hasUIDataId;
		
		public virtual bool TryGetUIDataId(out string uiDataId) =>
			_hasUIDataId.TryGetValue(out uiDataId);

		
		[Preserve]
		public OverrideHasUIDataId() { }
	}
}