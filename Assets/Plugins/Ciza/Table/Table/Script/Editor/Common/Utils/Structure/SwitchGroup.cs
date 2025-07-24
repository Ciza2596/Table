using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public class SwitchGroup
	{
		public string GroupName { get; }

		public int SiblingIndex { get; set; }
		public SerializedProperty Leader { get; set; }

		[field: NonSerialized]
		public List<SwitchGroupMember> Members { get; } = new List<SwitchGroupMember>();

		[Preserve]
		public SwitchGroup(string groupName)
		{
			GroupName = groupName;
		}

		public List<SerializedProperty> GetMembersByValue()
		{
			if (Leader.propertyType == SerializedPropertyType.Enum)
				return Members.Where(x => CheckMemberByAssociatedValue_Enum(x, Leader.enumValueFlag)).Select(x => x.Member).ToList();

			return Members.Where(x => CheckMemberByAssociatedValue_Object(x, Leader.boxedValue)).Select(x => x.Member).ToList();
		}

		private bool CheckMemberByAssociatedValue_Enum(SwitchGroupMember member, int leaderValue)
		{
			var valueType = member.AssociatedValue.GetType();
			if (!valueType.IsEnum && !valueType.IsArray)
				return false;

			if (member.AssociatedValue is object[] values)
				return member.IsInverted ? values.All(value => (int)value != leaderValue) : values.Any(value => (int)value == leaderValue);

			var intValue = (int)(member.AssociatedValue);
			return member.IsInverted ? intValue != leaderValue : intValue == leaderValue;
		}

		private bool CheckMemberByAssociatedValue_Object(SwitchGroupMember member, object leaderValue)
		{
			if (member.AssociatedValue is object[] values)
				return member.IsInverted ? values.All(value => value.Equals(leaderValue)) : values.Any(value => value.Equals(leaderValue));

			return member.IsInverted ? !member.AssociatedValue.Equals(leaderValue) : member.AssociatedValue.Equals(leaderValue);
		}

		public class SwitchGroupMember
		{
			public SerializedProperty Member { get;  }
			public object AssociatedValue { get; }
			public bool IsInverted { get;  }

			[Preserve]
			public SwitchGroupMember(SerializedProperty property, object value, bool isInverted)
			{
				Member = property;
				AssociatedValue = value;
				IsInverted = isInverted;
			}
		}
	}
}