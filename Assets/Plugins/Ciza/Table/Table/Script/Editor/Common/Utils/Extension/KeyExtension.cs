using UnityEngine;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public static class KeyExtension
	{
		// Windows-ctrl, OSX-command
		public static bool CheckIsCtrl(this EventModifiers modifier) =>
			(Application.platform == RuntimePlatform.WindowsEditor && (modifier & EventModifiers.Control) != 0) || (Application.platform == RuntimePlatform.OSXEditor && (modifier & EventModifiers.Command) != 0);
	}
}