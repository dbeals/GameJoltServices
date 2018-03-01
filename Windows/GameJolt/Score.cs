using System;
using GameJolt.Async;
using GameJolt.Requests;
using Newtonsoft.Json;

namespace GameJolt
{
	[JsonObject]
	public sealed class Score
	{
		[JsonProperty("score")]
		public string Value { get; set; }

		[JsonProperty("sort")]
		public string Sort { get; set; }

		[JsonProperty("extra_data")]
		public string ExtraData { get; set; }

		[JsonProperty("user")]
		public string User { get; set; }

		[JsonProperty("user_id")]
		public string UserId { get; set; }

		[JsonProperty("guest")]
		public string Guest { get; set; }

		[JsonProperty("stored")]
		public string Stored { get; set; }

		public static IAsyncResult BeginGetScores(string gameId, string privateKey, string username, string userToken, int limit, string tableId, AsyncCallback callback = null, object asyncState = null)
		{
			var request = ScoreRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, limit, tableId);
			return request.Begin(callback, asyncState);
		}

		public static Score[] EndGetScores(IAsyncResult result)
		{
			var joltResult = (AsyncResult<Score[]>) result;
			var request = (JsonRequest<Score[], ScoreRequestFactory.ScoreRequestResult>) joltResult.CoreData;
			return request.End(result);
		}

		public static Score[] GetScores(string gameId, string privateKey, string username, string userToken, int limit, string tableId)
		{
			var request = ScoreRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, limit, tableId);
			return request.Process(null);
		}

		public static IAsyncResult GetScores(string gameId, string privateKey, string username, string userToken, int limit, string tableId, Action<Score[]> callback)
		{
			return BeginGetScores(gameId, privateKey, username, userToken, limit, tableId, result => callback(EndGetScores(result)));
		}

		public static IAsyncResult BeginAddScore(string gameId, string privateKey, string score, int sort, string username, string userToken, string guest, string extraData, string tableId, AsyncCallback callback = null, object asyncState = null)
		{
			var request = ScoreRequestFactory.CreateAddRequest(gameId, privateKey, score, sort, username, userToken, guest, extraData, tableId);
			return request.Begin(callback, asyncState);
		}

		public static string EndAddScore(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static string AddScore(string gameId, string privateKey, string score, int sort, string username, string userToken, string guest, string extraData, string tableId)
		{
			var request = ScoreRequestFactory.CreateAddRequest(gameId, privateKey, score, sort, username, userToken, guest, extraData, tableId);
			return request.Process(null);
		}

		public IAsyncResult AddScore(string gameId, string privateKey, string score, int sort, string username, string userToken, string guest, string extraData, string tableId, Action<string> callback)
		{
			return BeginAddScore(gameId, privateKey, score, sort, username, userToken, guest, extraData, tableId, result => callback(EndAddScore(result)));
		}
	}
}