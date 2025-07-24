using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public class NormalizedEnabler : BEnabler<float>
	{
		// VARIABLE: -----------------------------------------------------------------------------

		[Range(0, 1)]
		[SerializeField]
		protected float _value;

		protected override float ValueImp
		{
			get => _value;
			set => _value = value;
		}

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public NormalizedEnabler() { }

		[Preserve]
		public NormalizedEnabler(bool isEnable) =>
			_isEnable = isEnable;

		[Preserve]
		public NormalizedEnabler(float value) : base(value) { }

		[Preserve]
		public NormalizedEnabler(bool isEnable, float value) : base(isEnable, value) { }
	}
}