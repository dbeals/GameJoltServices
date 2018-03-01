using System;
using System.Threading;

namespace GameJolt.Async
{
	public class AsyncResultNoResult : IAsyncResult
	{
		public enum TaskState
		{
			Pending = 0,
			CompletedSynchronously,
			CompletedAsynchronously,
			Failed
		}

		private readonly AsyncCallback _asyncCallback;
		private int _completedState = (int) TaskState.Pending;
		private ManualResetEvent _asyncWaitHandle;

		public object AsyncState { get; }

		public bool CompletedSynchronously => Thread.VolatileRead(ref _completedState) == (int) TaskState.CompletedSynchronously;

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (_asyncWaitHandle != null)
					return _asyncWaitHandle;

				var done = IsCompleted;
				var resetEvent = new ManualResetEvent(done);
				if (Interlocked.CompareExchange(ref _asyncWaitHandle, resetEvent, null) != null)
					resetEvent.Close();
				else
				{
					if (!done && IsCompleted)
						_asyncWaitHandle.Set();
				}

				return _asyncWaitHandle;
			}
		}

		public bool IsCompleted => Thread.VolatileRead(ref _completedState) != (int) TaskState.Pending;
		public object CoreData { get; private set; }
		public Exception Exception { get; set; }

		public AsyncResultNoResult(AsyncCallback asyncCallback, object asyncState, object coreData)
		{
			_asyncCallback = asyncCallback;
			AsyncState = asyncState;
			CoreData = coreData;
		}

		public void SetAsCompleted(Exception exception, bool completedSynchronously)
		{
			Exception = exception;

			var previousState = (TaskState) Interlocked.Exchange(ref _completedState, completedSynchronously ? (int) TaskState.CompletedSynchronously : (int) TaskState.CompletedAsynchronously);
			if (previousState == TaskState.Failed)
				return;

			if (previousState != TaskState.Pending)
				throw new InvalidOperationException("You can only call SetAsCompleted once.");

			_asyncWaitHandle?.Set();

			try
			{
				_asyncCallback?.Invoke(this);
			}
			catch (Exception)
			{
				// Set the state to failed so if an exception is thrown during asyncCallback we don't cause another exception when SetAsCompleted() is called again.
				_completedState = (int) TaskState.Failed;
				throw;
			}
		}

		public void EndInvoke()
		{
			if (!IsCompleted)
			{
				AsyncWaitHandle.WaitOne();
				AsyncWaitHandle.Close();
				_asyncWaitHandle = null;
			}

			if (Exception != null)
				throw Exception;
		}
	}
}