using System;

namespace GameJolt.Async
{
	public class AsyncResult<TValue> : AsyncResultNoResult
	{
		private TValue _result;

		public AsyncResult(AsyncCallback asyncCallback, object state, object coreData)
			: base(asyncCallback, state, coreData) { }

		public void SetAsCompleted(TValue result, bool completedSynchronously)
		{
			_result = result;
			SetAsCompleted(null, completedSynchronously);
		}

		public new TValue EndInvoke()
		{
			base.EndInvoke();
			return _result;
		}
	}
}