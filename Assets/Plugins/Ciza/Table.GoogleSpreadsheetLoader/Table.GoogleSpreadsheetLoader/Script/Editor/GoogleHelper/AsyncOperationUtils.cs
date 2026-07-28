using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace CizaTable.Editor
{
	public static class AsyncOperationUtils
	{
		// PUBLIC METHOD: ----------------------------------------------------------------------

		public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOperation)
		{
			var taskCompletionSource = new TaskCompletionSource<object>();
			asyncOperation.completed += _ => { taskCompletionSource.SetResult(null); };
			return ((Task)taskCompletionSource.Task).GetAwaiter();
		}
	}
}