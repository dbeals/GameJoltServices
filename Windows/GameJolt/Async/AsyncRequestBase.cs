using System;

namespace GameJolt.Async
{
	public abstract class AsyncRequestBase<TResult>
	{
		public virtual string GameId { get; set; }

		protected virtual string PrivateKey { get; set; }

		protected AsyncRequestBase(string gameId, string privateKey)
		{
			if (string.IsNullOrWhiteSpace(gameId))
				return;

			GameId = gameId;
			PrivateKey = privateKey;
		}

		public virtual IAsyncResult Begin(AsyncCallback callback, object asyncState)
		{
			return AsyncHelper.BeginAsyncCall<TResult>(Process, callback, asyncState, this);
		}

		public virtual TResult End(IAsyncResult result)
		{
			return AsyncHelper.EndAsyncCall<TResult>(result);
		}

		public virtual TResult Process(IAsyncResult result)
		{
			return default(TResult);
		}
	}
}