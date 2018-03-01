using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameJolt.Async
{
	internal static class AsyncHelper
	{
		public static AsyncResult<TResult> BeginAsyncCall<TResult>(Func<AsyncResult<TResult>, TResult> method, AsyncCallback callback, object asyncState, object coreData)
		{
			var outputAsyncResult = new AsyncResult<TResult>(callback, asyncState, coreData);
			ThreadPool.QueueUserWorkItem(data =>
			{
				var asyncResult = (AsyncResult<TResult>) data;
				try
				{
					var outputValue = method(asyncResult);
					asyncResult.SetAsCompleted(outputValue, false);
				}
				catch (Exception exception)
				{
					asyncResult.SetAsCompleted(exception, false);
				}
			}, outputAsyncResult);
			return outputAsyncResult;
		}

		public static TResult EndAsyncCall<TResult>(IAsyncResult result)
		{
			var tempResult = (AsyncResult<TResult>) result;
			return tempResult.EndInvoke();
		}
	}
}