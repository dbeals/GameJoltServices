using System;
using GameJolt.Async;
using GameJolt.Requests;

namespace GameJolt
{
	public sealed class Session
	{
		public string Username { get; set; }
		public string UserToken { get; set; }
		public SessionStatus Status { get; set; } = SessionStatus.Unknown;

		public bool IsValid { get; private set; }

		public Session(string username, string userToken)
		{
			Username = username;
			UserToken = userToken;
		}

		public IAsyncResult BeginOpen(string gameId, string privateKey, AsyncCallback callback = null, object asyncState = null)
		{
			var request = SessionRequestFactory.CreateOpenRequest(gameId, privateKey, Username, UserToken);
			return request.Begin(callback, asyncState);
		}

		public string EndOpen(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			var data = request.End(result);
			IsValid = RequestHelper.WasSuccessful(data);
			return data;
		}

		public string Open(string gameId, string privateKey)
		{
			var request = SessionRequestFactory.CreateOpenRequest(gameId, privateKey, Username, UserToken);
			var data = request.Process(null);
			IsValid = RequestHelper.WasSuccessful(data);
			return data;
		}

		public IAsyncResult Open(string gameId, string privateKey, Action<string> callback)
		{
			return BeginOpen(gameId, privateKey, result =>
			{
				var data = EndOpen(result);
				IsValid = RequestHelper.WasSuccessful(data);
				callback(data);
			});
		}

		public IAsyncResult BeginPing(string gameId, string privateKey, AsyncCallback callback = null, object asyncState = null)
		{
			var request = SessionRequestFactory.CreatePingRequest(gameId, privateKey, Username, UserToken, Status);
			return request.Begin(callback, asyncState);
		}

		public string EndPing(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			var data = request.End(result);
			return data;
		}

		public string Ping(string gameId, string privateKey)
		{
			var request = SessionRequestFactory.CreatePingRequest(gameId, privateKey, Username, UserToken, Status);
			return request.Process(null);
		}

		public IAsyncResult Ping(string gameId, string privateKey, Action<string> callback)
		{
			return BeginPing(gameId, privateKey, result => callback(EndPing(result)));
		}

		public IAsyncResult BeginClose(string gameId, string privateKey, AsyncCallback callback = null, object asyncState = null)
		{
			var request = SessionRequestFactory.CreateCloseRequest(gameId, privateKey, Username, UserToken);
			return request.Begin(callback, asyncState);
		}

		public string EndClose(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			var data = request.End(result);
			Status = SessionStatus.Closed;
			IsValid = false;
			return data;
		}

		public string Close(string gameId, string privateKey)
		{
			var request = SessionRequestFactory.CreateCloseRequest(gameId, privateKey, Username, UserToken);
			return request.Process(null);
		}

		public IAsyncResult Close(string gameId, string privateKey, Action<string> callback)
		{
			return BeginClose(gameId, privateKey, result => callback(EndClose(null)));
		}
	}
}