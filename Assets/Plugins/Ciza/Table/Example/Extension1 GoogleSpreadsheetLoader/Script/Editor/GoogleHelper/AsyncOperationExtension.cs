using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace GoogleHelper
{
	public static class AsyncOperationExtension
	{
		public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOperation)
		{
			var taskCompletionSource = new TaskCompletionSource<object>();
			asyncOperation.completed += obj => { taskCompletionSource.SetResult(null); };
			return ((Task)taskCompletionSource.Task).GetAwaiter();
		}
	}
}
